using CommonDomain.Messaging;
using CommonDomain.Wireup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CartExample.Infrastructure
{
    public sealed class DomainServices
    {
        // Private object with lazy instantiation
        private static readonly Lazy<DomainServices> instance =
            new Lazy<DomainServices>( () => { return new DomainServices(); }) ;

        public IDependencyResolver Resolver { set { this.resolver = value; } }
        IDependencyResolver resolver;      
    
        public IEventPublisher EventPublisher { get { return this.resolver.GetInstance<IEventPublisher>(); }}

        private DomainServices() { } // no public default constructor                  
        public static DomainServices Instance // static instance property
        {
            get { return instance.Value; }
        }
    }
}
