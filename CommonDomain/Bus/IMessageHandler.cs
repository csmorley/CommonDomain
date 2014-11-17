using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonDomain.Bus
{
    internal interface IMessageHandler
    {
        string HandlerName { get; }
        bool TryHandle(Message message);
        bool IsSame<T>(object handler);
    }
}
