using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonDomain.Bus
{
    public interface ISubscriber
    {
        void Subscribe<T>(IHandle<T> handler) where T : Message;
        void Unsubscribe<T>(IHandle<T> handler) where T : Message;
    }
}
