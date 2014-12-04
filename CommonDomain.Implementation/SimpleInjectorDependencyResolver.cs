using CommonDomain.Wireup;
using CommonDomain.Implementation;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonDomain.Implementation
{
    public class SimpleInjectorDependencyResolver : IDependencyResolver
    {
        private readonly Container container;

        public void Register<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService
        {
            this.container.Register<TService, TImplementation>();
        }
        public SimpleInjectorDependencyResolver(Container container)
        {
            this.container = container;
        }

        public TService GetInstance<TService>() where TService : class
        {
            return this.container.GetInstance<TService>();
        }

        public object GetInstance(Type type)
        {
            return this.container.GetInstance(type);
        }

        public IEnumerable<T> GetInstances<T>() where T : class
        {
            return this.container.GetAllInstances<T>();
        }

        public IEnumerable<object> GetInstances(Type type)
        {
            return this.container.GetAllInstances(type);
        }
    }
}
