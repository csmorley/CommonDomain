using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonDomain.Messaging
{
    public interface IExternalEventConverter
    {
        string Name { get; }
        Version SupportsUpTo { get; }
        string BoundedContextName { get; }
        string ConvertsFromName { get; }
        Type ConvertsToType { get; }
        object Convert(byte[] payload);
    }
}
