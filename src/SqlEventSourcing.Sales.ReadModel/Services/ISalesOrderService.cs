using SqlEventSourcing.Sales.SharedKernel.CustomTypes;
using SqlEventSourcing.Shared.Contracts;
using SqlEventSourcing.Shared.CustomTypes;
using SqlEventSourcing.Shared.DomainIds;
using SqlEventSourcing.Shared.Entities;

namespace SqlEventSourcing.Sales.ReadModel.Services;

public interface ISalesOrderService
{
	Task CreateSalesOrderAsync(SalesOrderId salesOrderId, SalesOrderNumber salesOrderNumber, CustomerId customerId,
		CustomerName customerName, OrderDate orderDate, IEnumerable<SalesOrderRowDto> rows, CancellationToken cancellationToken);

	Task<PagedResult<SalesOrderJson>> GetSalesOrdersAsync(int page, int pageSize, CancellationToken cancellationToken);
	Task CompleteSalesOrderAsync(SalesOrderId eventSalesOrderId, CancellationToken cancellationToken);
	Task SetSalesOrderDeliveryDateAsync(SalesOrderId eventSalesOrderId, DeliveryDate eventDeliveryDate, CancellationToken cancellationToken);
}