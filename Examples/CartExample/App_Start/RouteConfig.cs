using CartExample.Domain.Carts;
using CartExample.Domain.Products;
using CartExample.Infrastructure;
using CartExample.Mock;
using CommonDomain.Aggregates;
using CommonDomain.Implementation;
using CommonDomain.Mediator;
using CommonDomain.Messaging;
using SimpleInjector;
using SimpleInjector.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CartExample.Web
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
        }
    }
}
