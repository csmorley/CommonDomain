using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonDomain.Messaging;

namespace CommonDomain.Persistence
{
    public interface IEventStore
    {
        void SubscribeToAllFrom(int position, Action<EventInfo> eventAppeared, Action liveProcessingStarted, Action<SubscriptionDropReason, Exception> subscriptionDropped);
    }
}
