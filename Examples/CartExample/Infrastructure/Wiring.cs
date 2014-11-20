using CommonDomain.Mediator;
using CommonDomain.Persistence;
using EventStore.ClientAPI;
using ForUs.Common.Domain.Repositories;
using SimpleInjector;
using SimpleInjector.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CartExample.Infrastructure
{
    public class Wiring
    {
        public static void WireUp(Container container)
        {
            container.RegisterSingle<Database>();
            container.RegisterSingle<IRepository, GetEventStoreRepository>();
            container.RegisterSingle<IStreamNamingConvention, StreamNamingConvention>();
            container.RegisterSingle<IEventStoreConnection>( () => { return EventStoreConnectionManager.Init(); } );

            container.RegisterManyForOpenGeneric(typeof(IEventHandler<>),
                AccessibilityOption.AllTypes,
                (serviceType, implTypes) => container.RegisterAll(serviceType, implTypes),
                AppDomain.CurrentDomain.GetAssemblies()
            );

            container.Register<IQueryHandler<NameTestQuery, int>, TestQueryHandler>();

            container.Verify();
        }
    }
}