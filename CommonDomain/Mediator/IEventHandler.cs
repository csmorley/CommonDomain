using CommonDomain.Aggregates;
using CommonDomain.Messaging;

namespace CommonDomain.Mediator
{
    public interface IEventHandler<in TEvent> where TEvent : class
    {
        void Handle(Identity senderId, TEvent eventToHandle, bool isReplay);
    }
}
