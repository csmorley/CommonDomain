using CommonDomain.Bus;
using CommonDomain.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonDomain.Wireup
{
    class Wiring
    {
        public Wiring(
            IEventStore eventStore,
            IBus internalBus,
            IPublisher externalEventPublisher,
            ISubscriber externalEventSubscriber)
        {
            //var dispatcher = new EventStoreDispatcher(externalEventPublisher, eventStore);
        }
    }
}
