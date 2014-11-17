using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using CommonDomain.Wireup;
using CommonDomain.Messaging;

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


        public Result PublishEvent(object eventToPublish, bool isReplay)
        {            
            var type = (typeof(IEventHandler<>)).MakeGenericType(eventToPublish.GetType());
            var handlers = resolver.GetInstances(type); // get instances, we will push to many listeners
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
                        new[] { eventToPublish.GetType(), typeof(bool) },
                        null);

                    method.Invoke(handler, new[] { eventToPublish, isReplay });
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

        public Result PublishEvent<TEvent>(TEvent eventToPublish, bool isReplay) where TEvent: class
        {
            var handlers = this.resolver.GetInstances<IEventHandler<TEvent>>();  // get instances, we will push to many listeners
            var response = new Result();
            List<Exception> exceptions = null;

            foreach (var handler in handlers)
            {
                try
                {
                    handler.Handle(eventToPublish, isReplay);
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