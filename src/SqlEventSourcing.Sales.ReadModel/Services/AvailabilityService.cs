using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Muflone.Persistence.Sql.Persistence;
using SqlEventSourcing.Sales.ReadModel.Facade;
using SqlEventSourcing.Shared.CustomTypes;
using SqlEventSourcing.Shared.DomainIds;

namespace SqlEventSourcing.Sales.ReadModel.Services;

public sealed class AvailabilityService(ILoggerFactory loggerFactory,
	SalesDbContext salesDbContext,
	SqlOptions sqlOptions) : ServiceBase(loggerFactory, salesDbContext, sqlOptions), IAvailabilityService
{
	public async Task UpdateAvailabilityAsync(BeerId beerId, BeerName beerName, Quantity quantity,
		CancellationToken cancellationToken = default)
	{
		cancellationToken.ThrowIfCancellationRequested();
		
		try
		{
			await Task.Run(async () =>
			{
				var availability = await SalesDbContext.Availabilities
					.AsNoTracking()
					.FirstOrDefaultAsync(o => o.Id == beerId.Value, cancellationToken);

				if (availability == null)
				{
					availability = Dtos.Availability.Create(beerId, beerName, quantity);
					await SalesDbContext.AddAsync(availability, cancellationToken);
				}
				else
				{
					availability.UpdateAvailability(quantity);
				}

				await SalesDbContext.SaveChangesAsync(cancellationToken);
			}, cancellationToken);
		}
		catch (Exception ex)
		{
			Logger.LogError(ex, "Error updating availability");
			throw;
		}
	}
}