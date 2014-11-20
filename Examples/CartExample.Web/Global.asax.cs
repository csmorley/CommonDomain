﻿using CartExample.Domain.Carts;
using CartExample.Domain.Products;
using CartExample.Projections;
using CartExample.Infrastructure;
using CartExample.Mock;
using CommonDomain.Aggregates;
using CommonDomain.Implementation;
using CommonDomain.Mediator;
using CommonDomain.Persistence;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace CartExample.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var container = new Container();
            Wiring.WireUp(container);
            DependencyResolver.SetResolver(new SimpleInjector.Integration.Web.Mvc.SimpleInjectorDependencyResolver(container));

            var product1 = new Product(new ProductId("11-11-1111"), "Book 1");
            var product2 = new Product(new ProductId("22-22-2222"), "Book 2");
            var product3 = new Product(new ProductId("33-33-3333"), "Book 3");
            var product4 = new Product(new ProductId("44-44-4444"), "Book 4");
            var product5 = new Product(new ProductId("55-55-5555"), "Book 5");
            var product6 = new Product(new ProductId("66-66-6666"), "Book 6");
            var product7 = new Product(new ProductId("77-77-7777"), "Book 7");
            var product8 = new Product(new ProductId("88-88-8888"), "Book 8");
            var products = new List<Product>(new[] { product1, product2, product3, product4, product5, product6, product7, product8 });

            var now = DateTime.UtcNow;
            var exampleCart = new Cart(new CartId(Guid.NewGuid()), now, now);
            exampleCart.AddToCart(product1, 1);
            exampleCart.AddToCart(product1, 1);
            exampleCart.RemoveFromCart(product1, -1);
            exampleCart.AddToCart(product1, 10);

            exampleCart.AddToCart(product2, 10);
            exampleCart.RemoveFromCart(product2, -10);

            exampleCart.AddToCart(product3, 5);

            exampleCart.AddToCart(product4, 3);
            exampleCart.RemoveFromCart(product4, -2);

            exampleCart.Checkout();

            var repository = container.GetInstance<IRepository>();
            repository.Save(exampleCart);

            var testData = new TestData(products).Create();

            var mediator = new Mediator(new SimpleInjectorDependencyResolver(container));

            //mediator.RequestQuery(new NameTestQuery() { Name = "Chris" });

            int count = 0;
            var start = DateTime.UtcNow;
            foreach(var cart in testData)
            {
                count = count + (cart as IAggregate).GetUncommittedEvents().Count;
                repository.Save(cart);
                //var result = DispatchEvents.Dispatch(cart, mediator);
                //count =+ count;
            }
            var duration = DateTime.UtcNow - start;
            var eventsPerSecond = (double)count / duration.TotalSeconds;

            // now view the read model for projected streams
            var database = container.GetInstance<Database>();

            var moreThanOneItemAbandoned = database.CartsWithAbandonedItems.Where(x => x.Value.Count > 1);


            /*var aggregate = cart as IAggregate;
            var events = aggregate.GetUncommittedEvents();
            foreach(var e in events)
            {
                var envelope = new EventEnvelope(aggregate, e);
                mediator.PublishEvent(envelope, false);
            }*/

            /*container.RegisterAll<IEventHandler<NewUserAddedEvent>>(
                typeof(UpdateNewUserDashboard),
                typeof(SendNewUserEmail)
                );

            container.Register<ICommandHandler<NewUserCommand>, NewUserCommandHandler>();

            container.Register<IQueryHandler<GetUsernameQuery, string>, GetUsernameQueryHandler>();*/
        }
    }
}
