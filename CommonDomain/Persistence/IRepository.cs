using CommonDomain.Aggregates;
using System;
using System.Collections.Generic;

namespace CommonDomain.Persistence
{
	public interface IRepository
	{
	    TAggregate GetById<TAggregate>(Guid id) where TAggregate : class, IAggregate;
		TAggregate GetById<TAggregate>(Guid id, int version) where TAggregate : class, IAggregate;
        void Save(IAggregate aggregate, Guid commitId, Action<IDictionary<string, object>> updateHeaders);
        void Save(IAggregate aggregate);
	}
}