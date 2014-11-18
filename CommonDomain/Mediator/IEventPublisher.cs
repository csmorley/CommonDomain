using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonDomain.Messaging;

namespace CommonDomain.Mediator
{
    public interface IEventPublisher
    {
        Result PublishEvent(EventEnvelope envelope, bool isReplay);
    }
}
