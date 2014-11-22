using EventStore.ClientAPI.SystemData;
using EventStore.ClientAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CommonDomain.Messaging;
using Newtonsoft.Json.Linq;

namespace CommonDomain.Implementation
{
    public class GetEventStore
    {
        UserCredentials credentials;
        IEventStoreConnection connection;
        IEventPublisher eventPublisher;
        readonly JsonSerializerSettings serializerSettings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.None };
        readonly string eventClrTypeHeaderNameString = "EventClrTypeName";

        public IEventStoreConnection Connection { get { return this.connection; } }

        public GetEventStore(string username, string password, IEventPublisher eventPublisher)
        {
            this.credentials = new UserCredentials(username,password);
            this.eventPublisher = eventPublisher;
            var eventStoreAddress = new IPEndPoint(IPAddress.Loopback, 1113);
            this.connection = EventStoreConnection.Create(eventStoreAddress);
        }

        public void Connect()
        {
            this.connection.ConnectAsync().Wait();

            /*this.connection.SubscribeToAllFrom(
                Position.Start,
                false,
                OnEventAppeared,
                OnLiveProcessingStarted,
                OnSubscriptionDropped,
                this.credentials);*/
        }

        private object DeserializeEvent(byte[] metadata, byte[] data)
        {
            if (metadata.Length == 0 || data.Length == 0)
                return null;

            try
            {
                var eventClrTypeName = JObject.Parse(Encoding.UTF8.GetString(metadata)).Property(this.eventClrTypeHeaderNameString).Value;
                return JsonConvert.DeserializeObject(Encoding.UTF8.GetString(data), Type.GetType((string)eventClrTypeName));
            }
            catch(Exception e)
            {
                var description = e.Message;
            }
            return null;
        }

        void OnEventAppeared(EventStoreCatchUpSubscription subscription, ResolvedEvent resolvedEvent)
        {
            var deserializedEvent = DeserializeEvent(resolvedEvent.OriginalEvent.Metadata, resolvedEvent.OriginalEvent.Data);
        }

        void OnLiveProcessingStarted(EventStoreCatchUpSubscription subscription)
        {
        }

        void OnSubscriptionDropped(EventStoreCatchUpSubscription subscription, SubscriptionDropReason reason, Exception exception)
        {
        }
    }
}
