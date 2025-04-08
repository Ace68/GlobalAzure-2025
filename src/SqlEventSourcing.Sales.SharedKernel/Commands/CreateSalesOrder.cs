using Muflone.Messages.Commands;
using SqlEventSourcing.Sales.SharedKernel.CustomTypes;
using SqlEventSourcing.Shared.Contracts;
using SqlEventSourcing.Shared.CustomTypes;
using SqlEventSourcing.Shared.DomainIds;

namespace SqlEventSourcing.Sales.SharedKernel.Commands;

public class CreateSalesOrder(SalesOrderId aggregateId, Guid commitId, SalesOrderNumber salesOrderNumber,
		OrderDate orderDate, CustomerId customerId, CustomerName customerName,
		IEnumerable<SalesOrderRowDto> rows)
	: Command(aggregateId, commitId)
{
	public readonly SalesOrderId SalesOrderId = aggregateId;
	public readonly SalesOrderNumber SalesOrderNumber = salesOrderNumber;
	public readonly OrderDate OrderDate = orderDate;

	public readonly CustomerId CustomerId = customerId;
	public readonly CustomerName CustomerName = customerName;

	public readonly IEnumerable<SalesOrderRowDto> Rows = rows;
}