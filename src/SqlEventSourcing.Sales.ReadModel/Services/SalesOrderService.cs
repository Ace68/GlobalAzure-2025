using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Muflone.Persistence.Sql.Persistence;
using SqlEventSourcing.Sales.ReadModel.Dtos;
using SqlEventSourcing.Sales.ReadModel.Facade;
using SqlEventSourcing.Sales.SharedKernel.CustomTypes;
using SqlEventSourcing.Shared.Contracts;
using SqlEventSourcing.Shared.CustomTypes;
using SqlEventSourcing.Shared.DomainIds;
using SqlEventSourcing.Shared.Entities;
using SqlEventSourcing.Shared.ReadModel;

namespace SqlEventSourcing.Sales.ReadModel.Services;

public sealed class SalesOrderService(ILoggerFactory loggerFactory,
	SalesDbContext salesDbContext,
	SqlOptions sqlOptions, IQueries<SalesOrder> queries)
	: ServiceBase(loggerFactory, salesDbContext, sqlOptions), ISalesOrderService
{
	public async Task CreateSalesOrderAsync(SalesOrderId salesOrderId, SalesOrderNumber salesOrderNumber, CustomerId customerId,
		CustomerName customerName, OrderDate orderDate, IEnumerable<SalesOrderRowDto> rows, CancellationToken cancellationToken)
	{
		try
		{
			await using var dbContext = new SalesDbContext(SqlOptions);
			var existingOrder = await dbContext.SalesOrders
				.AsNoTracking()
				.FirstOrDefaultAsync(o => o.Id == salesOrderId.Value, cancellationToken);
				
			if (existingOrder != null)
				return;
				
			await dbContext.Database.BeginTransactionAsync(cancellationToken);
			var salesOrder = SalesOrder.CreateSalesOrder(salesOrderId, salesOrderNumber, customerId, customerName, orderDate, rows);
			SalesDbContext.SalesOrders.Add(salesOrder);
			await SalesDbContext.SaveChangesAsync(cancellationToken);
			await dbContext.Database.CommitTransactionAsync(cancellationToken);
		}
		catch (DbUpdateConcurrencyException ex)
		{
			Logger.LogError(ex, "Concurrency conflict occurred while creating sales order");
			throw;
		}
		catch (Exception ex)
		{
			Logger.LogError(ex, "Error creating sales order");
			throw;
		}
	}
	
	public async Task SetSalesOrderDeliveryDateAsync(SalesOrderId eventSalesOrderId, DeliveryDate deliveryDate,
		CancellationToken cancellationToken)
	{
		try
		{
			var salesOrder =
				await SalesDbContext.SalesOrders.FirstOrDefaultAsync(o => o.Id == eventSalesOrderId.Value,
					cancellationToken);
			salesOrder!.SetDeliveryDate(deliveryDate);
			await SalesDbContext.SaveChangesAsync(cancellationToken);
		}
		catch (Exception ex)
		{
			Logger.LogError(ex, "Error creating sales order");
			throw;
		}
	}

	public async Task<PagedResult<SalesOrderJson>> GetSalesOrdersAsync(int page, int pageSize, CancellationToken cancellationToken)
	{
		try
		{
			var salesOrders = await queries.GetByFilterAsync(null, page, pageSize, cancellationToken);

			// return salesOrders.TotalRecords > 0
			// 	? new PagedResult<SalesOrderJson>(salesOrders.Results.Select(r => r.ToJson()), salesOrders.Page, salesOrders.PageSize, salesOrders.TotalRecords)
			// 	: new PagedResult<SalesOrderJson>(Enumerable.Empty<SalesOrderJson>(), 0, 0, 0);
			return salesOrders.TotalRecords > 0
				? new PagedResult<SalesOrderJson>(salesOrders.Results.Select(r => r.ToJson()), salesOrders.Page, salesOrders.PageSize, salesOrders.TotalRecords)
				: new PagedResult<SalesOrderJson>(Enumerable.Empty<SalesOrderJson>(), 0, 0, 0);
		}
		catch (Exception ex)
		{
			Logger.LogError(ex, "Error reading SalesOrders");
			throw;
		}
	}

	public async Task CompleteSalesOrderAsync(SalesOrderId eventSalesOrderId, CancellationToken cancellationToken)
	{
		try
		{
			var salesOrder =
				await SalesDbContext.SalesOrders.FirstOrDefaultAsync(o => o.Id == eventSalesOrderId.Value,
					cancellationToken);
			salesOrder!.CompleteOrder();
			await SalesDbContext.SaveChangesAsync(cancellationToken);
		}
		catch (Exception ex)
		{
			Logger.LogError(ex, "Error completing SalesOrders");
			throw;
		}
	}
}