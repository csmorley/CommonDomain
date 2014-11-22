using EventStore.ClientAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonDomain.Implementation
{
    public class EventStoreSubscriptions
    {
        public Action<EventStoreCatchUpSubscription, ResolvedEvent> eventAppeared;
        public Action<EventStoreCatchUpSubscription> liveProcessingStarted;
        public Action<EventStoreCatchUpSubscription, SubscriptionDropReason, Exception> subscriptionDropped;
    }
}
