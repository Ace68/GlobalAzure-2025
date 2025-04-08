using Muflone.Messages.Events;
using SqlEventSourcing.Sales.SharedKernel.CustomTypes;
using SqlEventSourcing.Shared.CustomTypes;

namespace SqlEventSourcing.Sales.SharedKernel.Events;

public sealed class SalesOrderDeliveryDateSet(SalesOrderId aggregateId, Guid commitId, DeliveryDate deliveryDate)
    : DomainEvent(aggregateId, commitId)
{
    public readonly SalesOrderId SalesOrderId = aggregateId;
    public readonly DeliveryDate DeliveryDate = deliveryDate;
}