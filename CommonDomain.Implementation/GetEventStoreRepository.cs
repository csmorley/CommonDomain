﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonDomain.Aggregates;
using CommonDomain.Persistence;
using EventStore.ClientAPI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ForUs.Common.Domain.Repositories
{

    public class StreamNamingConvention : IStreamNamingConvention
    {
        public string GetStreamName(Type type, IIdentity identity)
        {
            return string.Format("{0}-{1}", char.ToLower(type.Name[0]) + type.Name.Substring(1), identity.IdentityValue);
        }
    }

    public interface IStreamNamingConvention
    {
        string GetStreamName(Type type, IIdentity identity);
    }

    public class GetEventStoreRepository : IRepository
    {
        private const string EventClrTypeHeader = "EventClrTypeName";
        private const string AggregateClrTypeHeader = "AggregateClrTypeName";
        private const string CommitIdHeader = "CommitId";
        private const int WritePageSize = 500;
        private const int ReadPageSize = 500;

        IStreamNamingConvention streamNamingConvention;

        private readonly IEventStoreConnection connection;
        private static readonly JsonSerializerSettings serializerSettings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.None };


        public GetEventStoreRepository(IEventStoreConnection eventStoreConnection, IStreamNamingConvention namingConvention)
        {
            this.connection = eventStoreConnection;
            this.streamNamingConvention = namingConvention;
        }

        public TAggregate GetById<TAggregate>(IIdentity id) where TAggregate : class, IAggregate
        {
            return GetById<TAggregate>(id, int.MaxValue);
        }

        public TAggregate GetById<TAggregate>(IIdentity id, int version) where TAggregate : class, IAggregate
        {
            if (version <= 0)
                throw new InvalidOperationException("Cannot get version <= 0");

            var streamName = this.streamNamingConvention.GetStreamName(typeof(TAggregate), id);
            var aggregate = ConstructAggregate<TAggregate>();

            var sliceStart = 0;
            StreamEventsSlice currentSlice;
            do
            {
                var sliceCount = sliceStart + ReadPageSize <= version
                                    ? ReadPageSize
                                    : version - sliceStart + 1;

                var readStreamTask = this.connection.ReadStreamEventsForwardAsync(streamName, sliceStart, sliceCount, false);
                readStreamTask.Wait();
                currentSlice = readStreamTask.Result;

                if (currentSlice.Status == SliceReadStatus.StreamNotFound)
                    throw new AggregateNotFoundException(id, typeof (TAggregate));
                
                if (currentSlice.Status == SliceReadStatus.StreamDeleted)
                    throw new AggregateDeletedException(id, typeof (TAggregate));
                
                sliceStart = currentSlice.NextEventNumber;

                foreach (var evnt in currentSlice.Events)
                    aggregate.ApplyEvent(DeserializeEvent(evnt.OriginalEvent.Metadata, evnt.OriginalEvent.Data));
            } while (version >= currentSlice.NextEventNumber && !currentSlice.IsEndOfStream);

            if (aggregate.Version != version && version < Int32.MaxValue)
                throw new AggregateVersionException(id, typeof (TAggregate), aggregate.Version, version);                

            return aggregate;
        }
        
        private static TAggregate ConstructAggregate<TAggregate>()
        {
            return (TAggregate)Activator.CreateInstance(typeof(TAggregate), true);
        }

        private static object DeserializeEvent(byte[] metadata, byte[] data)
        {
            var eventClrTypeName = JObject.Parse(Encoding.UTF8.GetString(metadata)).Property(EventClrTypeHeader).Value;
            return JsonConvert.DeserializeObject(Encoding.UTF8.GetString(data), Type.GetType((string)eventClrTypeName));
        }

        public void Save(IAggregate aggregate)
        {
            this.Save(aggregate, Guid.NewGuid(), d => { });

        }
        public void Save(IAggregate aggregate, Guid commitId, Action<IDictionary<string, object>> updateHeaders)
        {
            var commitHeaders = new Dictionary<string, object>
            {
                {CommitIdHeader, commitId},
                {AggregateClrTypeHeader, aggregate.GetType().AssemblyQualifiedName}
            };
            updateHeaders(commitHeaders);

            var streamName = this.streamNamingConvention.GetStreamName(aggregate.GetType(), aggregate.Identity);
            var newEvents = aggregate.GetUncommittedEvents().Cast<object>().ToList();
            var originalVersion = aggregate.Version - newEvents.Count;
            var expectedVersion = originalVersion == 0 ? ExpectedVersion.NoStream : originalVersion - 1;
            var eventsToSave = newEvents.Select(e => ToEventData(Guid.NewGuid(), e, commitHeaders)).ToList();

            if (eventsToSave.Count < WritePageSize)
            {
                this.connection.AppendToStreamAsync(streamName, expectedVersion, eventsToSave).Wait(); ;
            }
            else
            {
                var startTransactionTask = this.connection.StartTransactionAsync(streamName, expectedVersion);
                startTransactionTask.Wait();
                var transaction = startTransactionTask.Result;

                var position = 0;
                while (position < eventsToSave.Count)
                {
                    var pageEvents = eventsToSave.Skip(position).Take(WritePageSize);
                    var writeTask = transaction.WriteAsync(pageEvents);
                    writeTask.Wait();
                    position += WritePageSize;
                }

                var commitTask = transaction.CommitAsync();
                commitTask.Wait();
            }

            aggregate.ClearUncommittedEvents();
        }

        private static EventData ToEventData(Guid eventId, object evnt, IDictionary<string, object> headers)
        {
            var data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(evnt, serializerSettings));

            var eventHeaders = new Dictionary<string, object>(headers)
            {
                {
                    EventClrTypeHeader, evnt.GetType().AssemblyQualifiedName
                }
            };
            var metadata = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(eventHeaders, serializerSettings));
            var typeName = evnt.GetType().Name;

            return new EventData(eventId, typeName, true, data, metadata);
        }
    }
}

