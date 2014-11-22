using CommonDomain.Aggregates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonDomain.Implementation
{
    public class StreamNamingConvention : IStreamNamingConvention
    {
        public string GetStreamName(Type type, IIdentity identity)
        {
            return string.Format("{0}-{1}", char.ToLower(type.Name[0]) + type.Name.Substring(1), identity.Value);
        }
    }
}
