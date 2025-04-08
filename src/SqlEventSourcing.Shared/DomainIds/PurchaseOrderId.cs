using Muflone.Core;

namespace SqlEventSourcing.Shared.DomainIds;

public class PurchaseOrderId : DomainId
{
	public PurchaseOrderId(Guid value) : base(value.ToString())
	{
	}
}