using CartExample.Domain.Carts;
using CartExample.Domain.Products;
using CartExample.Projections;
using CartExample.Infrastructure;
using CartExample.Mock;
using CommonDomain.Aggregates;
using CommonDomain.Implementation;
using CommonDomain.Messaging;
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

            RepositoryExample.Run(container);
            ProjectionExample.Run(container);
            TweetsExample.Run(container);

            // now view the read model for projected streams
            var database = container.GetInstance<Database>();
            
            
        }
    }
}
