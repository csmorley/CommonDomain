using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonDomain.Wireup
{
    public interface IDependencyResolver
    {
        void Register<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService;
        T GetInstance<T>() where T : class;
        object GetInstance(Type type);
        IEnumerable<T> GetInstances<T>() where T : class;
        IEnumerable<object> GetInstances(Type type);
        
    }
}
