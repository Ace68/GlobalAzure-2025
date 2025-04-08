using Muflone.Messages.Events;
using SqlEventSourcing.Sales.SharedKernel.CustomTypes;

namespace SqlEventSourcing.Sales.SharedKernel.Events;

public sealed class SalesOrderClosed(SalesOrderId aggregateId, Guid correlationId) : DomainEvent(aggregateId, correlationId)
{
	public readonly SalesOrderId SalesOrderId = aggregateId;
}