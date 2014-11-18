using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using CommonDomain.Bus;
using CommonDomain.Aggregates;

namespace CommonDomain.Messaging
{
    public class EventEnvelope
    {
        public EventEnvelope(IAggregate owner, object eventObject)
        {
            //this.AggregateId = aggregateId;
            this.AggregateId = owner.Identity;

            this.EventObject = eventObject;
            this.EventDispatchedOn = DateTime.UtcNow;
            this.EventName = this.GetType().Name;
            // this.LibraryName = this.GetType().Namespace;
            this.EventContextName = ""; // Assembly.GetEntryAssembly().GetName().Name;
            this.EventVersion = this.GetType().Assembly.GetName().Version;
        }

        /*
        public DomainEvent(Guid aggregateId, DateTime occuredOn, string className, string contextName)
        {
            this.aggregateId = aggregateId;
            this.className = className;
            this.libraryNamspace = contextName;
            
            this.occuredOn = occuredOn;
        }
        */

        public IIdentity AggregateId { get; protected set; }
        public readonly object EventObject;
        public readonly string EventName;
        public readonly Version EventVersion;
        // public string LibraryName;
        public readonly string EventContextName;
        public readonly DateTime EventDispatchedOn;

    }
}
