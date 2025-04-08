using Muflone.Core;

namespace SqlEventSourcing.Shared.DomainIds;

public class SupplierId : DomainId
{
	public SupplierId(Guid value) : base(value.ToString())
	{
	}
}