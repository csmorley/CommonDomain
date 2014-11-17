using System;
using CommonDomain.Aggregates;

namespace CommonDomain.Persistence
{
	public interface IConstructAggregates
	{
		IAggregate Build(Type type, Guid id, IMemento snapshot);
	}
}