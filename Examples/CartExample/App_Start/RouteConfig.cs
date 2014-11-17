using CartExample.Domain.Cart;
using CartExample.Domain.Products;
using CommonDomain.Aggregates;
using CommonDomain.Implementation;
using CommonDomain.Mediator;
using SimpleInjector;
using SimpleInjector.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CartExample
{
    public class NameTestQuery : IQuery<int>
    {
        public string Name;
    }

    public class TestQueryHandler : IQueryHandler<NameTestQuery, int>
    {
        public int Handle(NameTestQuery queryToHandle)
        {
            return 1;
        }
    }

    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );


            var product1 = new Product(new ProductId("11-11-1111"));
            var product2 = new Product(new ProductId("22-22-2222"));
            var product3 = new Product(new ProductId("33-33-3333"));
            var product4 = new Product(new ProductId("44-44-4444"));

            var cart = new Cart("chris");
            
            cart.AddToCart(product1, 1);
            cart.AddToCart(product1, 1);
            cart.RemoveFromCart(product1, -1);
            cart.AddToCart(product1, 10);

            cart.AddToCart(product2, 10);
            cart.RemoveFromCart(product2, -10);

            cart.AddToCart(product3, 5);

            cart.AddToCart(product4, 3);
            cart.RemoveFromCart(product4, -2);

            cart.Checkout();

            var aggregate = cart as IAggregate;
            var events = aggregate.GetUncommittedEvents();

            var container = new Container();

            container.RegisterManyForOpenGeneric(typeof(IEventHandler<>),
                AccessibilityOption.AllTypes,
                (serviceType, implTypes) => container.RegisterAll(serviceType, implTypes),
                AppDomain.CurrentDomain.GetAssemblies()
            );

            container.Register<IQueryHandler<NameTestQuery, int>, TestQueryHandler>();           

            container.Verify();

            var mediator = new Mediator(new SimpleInjectorDependencyResolver(container));

            mediator.RequestQuery(new NameTestQuery() { Name = "Chris" });

            foreach(var e in events)
            {
                mediator.PublishEvent(e, false);
            }

            /*container.RegisterAll<IEventHandler<NewUserAddedEvent>>(
                typeof(UpdateNewUserDashboard),
                typeof(SendNewUserEmail)
                );

            container.Register<ICommandHandler<NewUserCommand>, NewUserCommandHandler>();

            container.Register<IQueryHandler<GetUsernameQuery, string>, GetUsernameQueryHandler>();*/
        }
    }
}
