using CommonDomain.Aggregates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonDomain.Implementation
{
    public interface IStreamNamingConvention
    {
        string GetStreamName(Type type, IIdentity identity);
    }
}
