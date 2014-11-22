using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonDomain.Messaging.Subscriptions;

namespace CommonDomain.Persistence
{
    public interface IEventStore
    {
        void Connect();
    }
}
