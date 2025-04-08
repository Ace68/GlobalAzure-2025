using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SqlEventSourcing.Sales.ReadModel.Dtos;
using SqlEventSourcing.Sales.ReadModel.Facade;
using SqlEventSourcing.Sales.ReadModel.Services;
using SqlEventSourcing.Shared.Entities;
using SqlEventSourcing.Shared.ReadModel;

namespace SqlEventSourcing.Sales.ReadModel.Queries;

public sealed class SalesOrderQueries(SalesDbContext salesDbContext) : IQueries<SalesOrder>
{
	public async Task<SalesOrder> GetByIdAsync(string id, CancellationToken cancellationToken)
	{
		SalesOrder? salesOrder =
			await salesDbContext.SalesOrders.FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
		
		return salesOrder != null
			? salesOrder
			: throw new KeyNotFoundException($"SalesOrder with Id {id} not found.");
	}

	public async Task<PagedResult<SalesOrder>> GetByFilterAsync(Expression<Func<SalesOrder, bool>>? query, int page, int pageSize, CancellationToken cancellationToken)
	{
		if (--page < 0)
			page = 0;

		var collection = salesDbContext.SalesOrders.AsQueryable();
		var queryable = query != null
			? collection.AsQueryable().Where(query)
			: collection.AsQueryable();

		var count = await queryable.CountAsync(cancellationToken: cancellationToken);
		var results = await queryable.Skip(page * pageSize).Take(pageSize).ToListAsync(cancellationToken: cancellationToken);

		return new PagedResult<SalesOrder>(results, page, pageSize, count);
	}
}