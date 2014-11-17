using CommonDomain.Bus;
using CommonDomain.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonDomain.Messaging
{
    public class EventDispatcher
    {
        public EventDispatcher(IPublisher publisher, IEventStore eventStore)
        {
            this.publisher = publisher;
            this.eventStore = eventStore;

            /* subscribe from last known position, calculate from EventStore Position */
            this.eventStore.SubscribeToAllFrom(
                0,
                EventAppeared,
                LiveProcessingStarted,
                SubscriptionDropped
                );
        }

        protected readonly IPublisher publisher;
        protected readonly IEventStore eventStore;

        public void EventAppeared(EventInfo eventInfo)
        {
            // open transaction
            // send on bus
            // write last sent (sequence + position) to database
            // commit transaction
        }

        private void SubscriptionDropped(SubscriptionDropReason reason, Exception exception)
        {
        }

        private void LiveProcessingStarted()
        {
        }
    }
}
