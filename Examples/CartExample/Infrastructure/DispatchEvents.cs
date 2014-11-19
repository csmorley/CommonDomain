using CommonDomain.Aggregates;
using CommonDomain.Mediator;
using CommonDomain.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CartExample.Infrastructure
{
    public class DispatchEvents
    {
        public static Tuple<ulong, TimeSpan> Dispatch(IAggregate aggregate, Mediator mediator)
        {
            var start = DateTime.UtcNow;
            var events = aggregate.GetUncommittedEvents();
            ulong count = 0;
            foreach (var e in events)
            {
                var envelope = new EventEnvelope(aggregate, e);
                mediator.PublishEvent(envelope, false);
                count++;
            }

            var duration = DateTime.UtcNow - start;

            return new Tuple<ulong, TimeSpan>(count, duration);
        }
    }
}