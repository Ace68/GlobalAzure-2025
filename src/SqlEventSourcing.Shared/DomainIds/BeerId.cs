using Muflone.Core;

namespace SqlEventSourcing.Shared.DomainIds;

public class BeerId : DomainId
{
	public BeerId(Guid value) : base(value.ToString())
	{
	}
}