using SqlEventSourcing.Shared.Contracts;
using SqlEventSourcing.Shared.Entities;

namespace SqlEventSourcing.Sales.Facade;

public interface ISalesFacade
{
	Task<string> CreateOrderAsync(SalesOrderJson body, CancellationToken cancellationToken);
	Task<PagedResult<SalesOrderJson>> GetOrdersAsync(CancellationToken cancellationToken);
	Task SetDeliveryDateAsync(string salesOrderId, CancellationToken cancellationToken);
}