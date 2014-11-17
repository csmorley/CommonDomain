using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonDomain.Bus
{
    public interface IPublisher
    {
        void Publish(Message message);
    }
}
