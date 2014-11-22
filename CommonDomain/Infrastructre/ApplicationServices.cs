using CommonDomain.Messaging;
using CommonDomain.Wireup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonDomain.Infrastructre
{
    public sealed class ApplicationServices
    {
        // Private object with lazy instantiation
        private static readonly Lazy<ApplicationServices> instance =
            new Lazy<ApplicationServices>(() => { return new ApplicationServices(); });

        public IDependencyResolver Resolver { set { this.resolver = value; } }
        IDependencyResolver resolver;

        public ICommandSender CommandSender { get { return this.resolver.GetInstance<ICommandSender>(); } }

        public IQuerySender QuerySender { get { return this.resolver.GetInstance<IQuerySender>(); } }

        private ApplicationServices() { } // no public default constructor                  
        public static ApplicationServices Instance // static instance property
        {
            get { return instance.Value; }
        }
    }
}
