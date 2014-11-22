using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonDomain.Messaging
{
    public interface IQuerySender
    {
        Result<TResponse> RequestQuery<TResponse>(IQuery<TResponse> queryToSend);
    }
}
