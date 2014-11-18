using System;
using System.Collections;

namespace CommonDomain.Aggregates
{	
	public interface IAggregate
	{
        IIdentity Identity { get; }
		int Version { get; }

		void ApplyEvent(object @event);
		ICollection GetUncommittedEvents();
		void ClearUncommittedEvents();

		IMemento GetSnapshot();
	}
}