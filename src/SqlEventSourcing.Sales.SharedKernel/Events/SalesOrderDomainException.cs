using Muflone.Messages.Events;
using SqlEventSourcing.Sales.SharedKernel.CustomTypes;

namespace SqlEventSourcing.Sales.SharedKernel.Events;

public sealed class SalesOrderDomainException(SalesOrderId aggregateId, Guid commitId, Exception exception)
    : DomainEvent(aggregateId, commitId)
{
    public readonly Exception Exception = exception;
}