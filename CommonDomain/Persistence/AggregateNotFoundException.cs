using CommonDomain.Aggregates;
using System;

namespace CommonDomain.Persistence
{
    public class AggregateNotFoundException : Exception
    {
        public readonly IIdentity Id;
        public readonly Type Type;

        public AggregateNotFoundException(IIdentity id, Type type)
            : base(string.Format("Aggregate '{0}' (type {1}) was not found.", id, type.Name))
        {
            Id = id;
            Type = type;
        }
    }
}