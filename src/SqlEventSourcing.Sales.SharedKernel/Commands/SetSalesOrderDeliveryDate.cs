using Muflone.Messages.Commands;
using SqlEventSourcing.Sales.SharedKernel.CustomTypes;
using SqlEventSourcing.Shared.CustomTypes;

namespace SqlEventSourcing.Sales.SharedKernel.Commands;

public sealed class SetSalesOrderDeliveryDate(SalesOrderId aggregateId, Guid commitId, DeliveryDate deliveryDate)
    : Command(aggregateId, commitId)
{
    public readonly SalesOrderId SalesOrderId = aggregateId;
    public readonly DeliveryDate DeliveryDate = deliveryDate;
}