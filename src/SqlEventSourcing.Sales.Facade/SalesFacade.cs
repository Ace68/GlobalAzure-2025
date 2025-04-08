using Muflone.Messages.Commands;
using Muflone.Persistence;
using SqlEventSourcing.Sales.ReadModel.Dtos;
using SqlEventSourcing.Sales.ReadModel.Services;
using SqlEventSourcing.Sales.SharedKernel.Commands;
using SqlEventSourcing.Sales.SharedKernel.CustomTypes;
using SqlEventSourcing.Shared.Contracts;
using SqlEventSourcing.Shared.CustomTypes;
using SqlEventSourcing.Shared.DomainIds;
using SqlEventSourcing.Shared.Entities;
using SqlEventSourcing.Shared.ReadModel;

namespace SqlEventSourcing.Sales.Facade;

public sealed class SalesFacade(IServiceBus serviceBus,
	IQueries<SalesOrder> orderQueries) : ISalesFacade
{
	public async Task<string> CreateOrderAsync(SalesOrderJson body, CancellationToken cancellationToken)
	{
		if (body.SalesOrderId.Equals(string.Empty))
			body = body with { SalesOrderId = Guid.NewGuid().ToString() };

		CreateSalesOrder command = new(new SalesOrderId(new Guid(body.SalesOrderId)),
						Guid.NewGuid(), new SalesOrderNumber(body.SalesOrderNumber), new OrderDate(body.OrderDate),
									new CustomerId(body.CustomerId), new CustomerName(body.CustomerName), body.Rows);
		await serviceBus.SendAsync(command, cancellationToken);

		return body.SalesOrderId;
	}
	
	public async Task SetDeliveryDateAsync(string salesOrderId, CancellationToken cancellationToken)
	{
		SetSalesOrderDeliveryDate command = new(new SalesOrderId(new Guid(salesOrderId)), Guid.NewGuid(),
			new DeliveryDate(DateTime.Now.AddDays(20)));
		await serviceBus.SendAsync(command, cancellationToken);
	}

	public async Task<PagedResult<SalesOrderJson>> GetOrdersAsync(CancellationToken cancellationToken)
	{
		var salesOrders = await orderQueries.GetByFilterAsync(null, 0, 100, cancellationToken);

		return salesOrders.TotalRecords > 0
			? new PagedResult<SalesOrderJson>(salesOrders.Results.Select(r => r.ToJson()), salesOrders.Page, salesOrders.PageSize, salesOrders.TotalRecords)
			: new PagedResult<SalesOrderJson>(Enumerable.Empty<SalesOrderJson>(), 0, 0, 0);
	}
}