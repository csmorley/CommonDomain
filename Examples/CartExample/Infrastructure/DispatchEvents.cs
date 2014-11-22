using CommonDomain.Aggregates;
using CommonDomain.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CartExample.Infrastructure
{
    public class DispatchEvents
    {
        public static ulong Dispatch(IAggregate aggregate, Mediator mediator)
        {
            var start = DateTime.UtcNow;
            var events = aggregate.GetUncommittedEvents();
            ulong count = 0;
            foreach (var e in events)
            {
                var envelope = new EventEnvelope(aggregate.Identity, e as IEvent);
                mediator.PublishEvent(envelope, false);
                count++;
            }           

            return count;
        }
    }
}