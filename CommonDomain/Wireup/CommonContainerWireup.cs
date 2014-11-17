using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonDomain.Wireup
{
    class CommonContainerWireup
    {
        public static void Wireup(IDependencyResolver container)
        {
            /*container1.RegisterManyForOpenGeneric(typeof(ICommandHandler<>),
                AccessibilityOption.AllTypes,
                (serviceType, implTypes) => container1.RegisterAll(serviceType, implTypes),
                AppDomain.CurrentDomain.GetAssemblies()
            );

            container.RegisterAll<IEventHandler<NewUserAddedEvent>>(
                typeof(UpdateNewUserDashboard),
                typeof(SendNewUserEmail)
                );

            container.Register<ICommandHandler<NewUserCommand>, NewUserCommandHandler>();

            container.Register<IQueryHandler<GetUsernameQuery, string>, GetUsernameQueryHandler>();
           */
        }
    }
}
