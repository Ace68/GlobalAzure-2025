using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SqlEventSourcing.Sales.ReadModel.Dtos;
using SqlEventSourcing.Sales.ReadModel.Facade;
using SqlEventSourcing.Sales.ReadModel.Services;
using SqlEventSourcing.Shared.Entities;
using SqlEventSourcing.Shared.ReadModel;

namespace SqlEventSourcing.Sales.ReadModel.Queries;

public sealed class AvailabilityQueries(SalesDbContext salesDbContext) : IQueries<Availability>
{
	public async Task<Availability> GetByIdAsync(string id, CancellationToken cancellationToken)
	{
		Availability? availability =
			await salesDbContext.Availabilities.FirstOrDefaultAsync(a => a.BeerId == id, cancellationToken);
		
		return availability != null
			? availability
			: throw new KeyNotFoundException($"Availability with BeerId {id} not found.");
	}

	public async Task<PagedResult<Availability>> GetByFilterAsync(Expression<Func<Availability, bool>>? query, int page, int pageSize, CancellationToken cancellationToken)
	{
		if (--page < 0)
			page = 0;

		var collection = salesDbContext.Availabilities.AsQueryable();
		var queryable = query != null
			? collection.AsQueryable().Where(query)
			: collection.AsQueryable();

		var count = await queryable.CountAsync(cancellationToken: cancellationToken);
		var results = await queryable.Skip(page * pageSize).Take(pageSize).ToListAsync(cancellationToken: cancellationToken);

		return new PagedResult<Availability>(results, page, pageSize, count);
	}
}