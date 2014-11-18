using System;

namespace CommonDomain.Aggregates
{
	public interface IMemento
	{
        IIdentity Id { get; set; }
		int Version { get; set; }
	}
}