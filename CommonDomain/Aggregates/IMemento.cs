using System;

namespace CommonDomain.Aggregates
{
	public interface IMemento
	{
		Guid Id { get; set; }
		int Version { get; set; }
	}
}