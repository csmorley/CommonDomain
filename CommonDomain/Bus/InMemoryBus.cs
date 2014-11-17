using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using CommonDomain.Utils;

namespace CommonDomain.Bus
{
    public class InMemoryBus : IBus, ISubscriber, IPublisher, IHandle<Message>
    {
        public string Name { get; private set; }

        private readonly Dictionary<Type, List<IMessageHandler>> _typeHash;

        private InMemoryBus() : this("Bus")
        {
        }

        public InMemoryBus(string name)
        {
            _typeHash = new Dictionary<Type, List<IMessageHandler>>();
            Name = name;
        }

        public void Subscribe<T>(IHandle<T> handler) where T : Message
        {
            Ensure.NotNull(handler, "handler");

            List<IMessageHandler> handlers;
            if (!_typeHash.TryGetValue(typeof(T), out handlers))
            {
                handlers = new List<IMessageHandler>();
                _typeHash.Add(typeof(T), handlers);
            }
            if (!handlers.Any(x => x.IsSame<T>(handler)))
                handlers.Add(new MessageHandler<T>(handler, handler.GetType().Name));
        }

        public void Unsubscribe<T>(IHandle<T> handler) where T : Message
        {
            Ensure.NotNull(handler, "handler");

            List<IMessageHandler> handlers;
            if (_typeHash.TryGetValue(typeof(T), out handlers))
            {
                var messageHandler = handlers.FirstOrDefault(x => x.IsSame<T>(handler));
                if (messageHandler != null)
                    handlers.Remove(messageHandler);
            }
        }

        public void Handle(Message message)
        {
            Publish(message);
        }

        public void Publish(Message message)
        {
            Ensure.NotNull(message,"message");
            DispatchByType(message);
        }

        private void DispatchByType(Message message)
        {
            var type = message.GetType();
            PublishByType(message, type);
            do
            {
                type = type.BaseType;
                PublishByType(message, type);
            } while (type != typeof(Message));
        }

        private void PublishByType(Message message, Type type)
        {
            List<IMessageHandler> handlers;
            if (_typeHash.TryGetValue(type, out handlers))
            {
                for (int i = 0, n = handlers.Count; i < n; ++i)
                {
                    var handler = handlers[i];
                    handler.TryHandle(message);
                }
            }
        }
    }
}
