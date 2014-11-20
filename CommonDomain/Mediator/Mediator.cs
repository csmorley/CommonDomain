using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using CommonDomain.Wireup;
using CommonDomain.Messaging;
using CommonDomain.Aggregates;

namespace CommonDomain.Mediator
{
    public class Mediator : IQuerySender, ICommandSender, IEventPublisher
    {
        readonly IDependencyResolver resolver;

        public Mediator(IDependencyResolver resolver)
        {
            this.resolver = resolver;
        }

        public virtual Result<TResponse> RequestQuery<TResponse>(IQuery<TResponse> queryToSend)
        {           
            var type = (typeof(IQueryHandler<,>)).MakeGenericType(queryToSend.GetType(), typeof(TResponse));
            var handler = resolver.GetInstance(type); // get single instance
            var response = new Result<TResponse>();

            try
            {
                MethodInfo method = handler.GetType().GetMethod(
                        "Handle",
                        BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod,
                        null,
                        CallingConventions.HasThis,
                        new[] { queryToSend.GetType() },
                        null);

                response.Data = (TResponse) method.Invoke(handler, new[] { queryToSend });
            }
            catch (Exception e)
            {
                response.Exception = e;
            }

            return response;
        }

        public Result SendCommand<TCommand>(TCommand commandToSend) where TCommand : class
        {
            var handler = resolver.GetInstance<ICommandHandler<TCommand>>(); // get single instance
            var response = new Result();            

            try
            {
                handler.Handle(commandToSend);
            }
            catch (Exception e)
            {
                response.Exception = e;
            }

            return response;
        }

        /*
        readonly Dictionary<Tuple<Type,Type>, MethodInfo> knownHandlerMethods = new Dictionary<Tuple<Type,Type>, MethodInfo>();

        MethodInfo GetEventHandlerMethodInfo(Type handlerType, Type eventType)
        {
            MethodInfo method;
            var key = new Tuple<Type,Type>(handlerType,eventType);

            if(this.knownHandlerMethods.TryGetValue(key, out method) == false)
            {
                method = handlerType.GetMethod(
                    "Handle",
                    BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod,
                    null,
                    CallingConventions.HasThis,
                    new[] { typeof(Identity), eventType, typeof(bool) },
                    null);

                this.knownHandlerMethods.Add(key, method);
            }

            return method;                
        }*/


        public Result PublishEvent(EventEnvelope envelope, bool isReplay)
        {
            var eventType = envelope.EventObject.GetType();
            var handlerType = (typeof(IEventHandler<>)).MakeGenericType(eventType);
            var handlers = resolver.GetInstances(handlerType); // get instances, we will push to many listeners
            var response = new Result();
            List<Exception> exceptions = null;

            foreach(var handler in handlers)
            {
                try
                {
                    MethodInfo method = handler.GetType().GetMethod(
                        "Handle",
                        BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod,
                        null,
                        CallingConventions.HasThis,
                        new[] { typeof(Identity), envelope.EventObject.GetType(), typeof(bool) },
                        null);

                    //var method = GetEventHandlerMethodInfo(handler.GetType(), envelope.EventObject.GetType());

                    method.Invoke(handler, new[] { envelope.AggregateId, envelope.EventObject, isReplay });
                }
                catch (Exception e)
                {
                    (exceptions ?? (exceptions = new List<Exception>())).Add(e);
                }
            }

            if (exceptions != null)
                response.Exception = new AggregateException(exceptions);

            return response;
        }
    }
}