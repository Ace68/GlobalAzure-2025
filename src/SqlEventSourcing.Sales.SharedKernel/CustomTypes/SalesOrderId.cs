using Muflone.Core;

namespace SqlEventSourcing.Sales.SharedKernel.CustomTypes;

public sealed class SalesOrderId : DomainId
{
	public SalesOrderId(Guid value) : base(value.ToString())
	{
	}
}