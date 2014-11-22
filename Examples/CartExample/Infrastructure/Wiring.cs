using CommonDomain.Messaging;
using CommonDomain.Persistence;
using EventStore.ClientAPI;
using SimpleInjector;
using SimpleInjector.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CartExample.Projections;
using CommonDomain.Implementation;
using CommonDomain.Wireup;
using CommonDomain.Infrastructre;

namespace CartExample.Infrastructure
{
    public class Wiring
    {

        public static void WireUp(Container container)
        {
            var eventStoreSubscriptions = new EventStoreSubscriptions();

            eventStoreSubscriptions.eventAppeared = (EventStoreCatchUpSubscription subscription, ResolvedEvent resolvedEvent) =>
            {
                var publisher = container.GetInstance<IEventPublisher>();
            };   

            // wireup the read model in-memory database
            container.RegisterSingle<Database>();
                                  
            // wireup event store and its repository
            container.RegisterSingle<GetEventStore>(() =>
            {
                return new GetEventStore( "admin", "changeit", container.GetInstance<IEventPublisher>());
            });
            container.RegisterSingle<IEventStoreConnection>(() =>
            {
                return container.GetInstance<GetEventStore>().Connection;
            });
            container.RegisterSingle<IStreamNamingConvention, StreamNamingConvention>();
            container.RegisterSingle<IRepository, GetEventStoreRepository>();

            // wireup IDependencyResolver which is used throughout the common domain code
            container.RegisterSingle<IDependencyResolver>(() => { return new SimpleInjectorDependencyResolver(container); });

            // register mediator and all the interfaces that it implements
            container.RegisterSingle<Mediator>();
            container.RegisterSingle<IQuerySender>(() => container.GetInstance<Mediator>());
            container.RegisterSingle<ICommandSender>(() => container.GetInstance<Mediator>());
            container.RegisterSingle<IEventPublisher>(() => container.GetInstance<Mediator>());

            // register all the event handlers in the assembly
            container.RegisterManyForOpenGeneric(typeof(IEventHandler<>),
                AccessibilityOption.AllTypes,
                (serviceType, implTypes) => container.RegisterAll(serviceType, implTypes),
                AppDomain.CurrentDomain.GetAssemblies()
            );

            // wireup any queries
            // container.Register<IQueryHandler<NameTestQuery, int>, TestQueryHandler>();

            container.Verify();

            //ApplicationServices.Instance.Resolver = container.GetInstance<IDependencyResolver>();
            //DomainServices.Instance.Resolver = container.GetInstance<IDependencyResolver>();
        }


    }
}