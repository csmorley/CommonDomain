using CommonDomain.Aggregates;
using System;

namespace CommonDomain.Persistence
{
    public class AggregateDeletedException : Exception
    {
        public readonly IIdentity Id;
        public readonly Type Type;

        public AggregateDeletedException(IIdentity id, Type type) 
            : base(string.Format("Aggregate '{0}' (type {1}) was deleted.", id, type.Name))
        {
            Id = id;
            Type = type;
        }
    }
}