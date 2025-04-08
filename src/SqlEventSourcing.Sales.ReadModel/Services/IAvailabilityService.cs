using SqlEventSourcing.Shared.CustomTypes;
using SqlEventSourcing.Shared.DomainIds;

namespace SqlEventSourcing.Sales.ReadModel.Services;

public interface IAvailabilityService
{
	Task UpdateAvailabilityAsync(BeerId beerId, BeerName beerName, Quantity quantity,
		CancellationToken cancellationToken = default);
}