using CommonDomain.Messaging;

namespace CommonDomain.Mediator
{
    public interface IEventHandler<in TEvent> where TEvent : class
    {
        void Handle(TEvent eventToHandle, bool isReplay);
    }
}
