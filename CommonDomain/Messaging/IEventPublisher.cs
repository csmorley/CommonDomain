using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonDomain.Messaging
{
    public interface IEventPublisher
    {
        Result PublishEvent(EventEnvelope envelope, bool isReplay);
    }
}
