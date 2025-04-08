using Muflone.Messages.Events;
using SqlEventSourcing.Sales.SharedKernel.CustomTypes;
using SqlEventSourcing.Shared.Contracts;
using SqlEventSourcing.Shared.CustomTypes;
using SqlEventSourcing.Shared.DomainIds;

namespace SqlEventSourcing.Sales.SharedKernel.Events;

public sealed class SalesOrderCreatedForIntegration(SalesOrderId aggregateId, Guid commitId, SalesOrderNumber salesOrderNumber,
    OrderDate orderDate, CustomerId customerId, CustomerName customerName,
    IEnumerable<SalesOrderRowDto> rows): IntegrationEvent(aggregateId, commitId)
{
    public readonly SalesOrderId SalesOrderId = aggregateId;
    public readonly SalesOrderNumber SalesOrderNumber = salesOrderNumber;
    public readonly OrderDate OrderDate = orderDate;

    public readonly CustomerId CustomerId = customerId;
    public readonly CustomerName CustomerName = customerName;

    public readonly IEnumerable<SalesOrderRowDto> Rows = rows;
}