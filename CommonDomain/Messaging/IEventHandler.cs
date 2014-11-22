using CommonDomain.Aggregates;

namespace CommonDomain.Messaging
{
    public interface IEventHandler<in TEvent> where TEvent : IEvent
    {
        void Handle(Identity senderId, TEvent eventToHandle, bool isReplay);
    }
}
