using Muflone.Messages.Commands;
using SqlEventSourcing.Shared.CustomTypes;
using SqlEventSourcing.Shared.DomainIds;

namespace SqlEventSourcing.Sales.SharedKernel.Commands;

public class UpdateAvailabilityDueToWarehousesNotification(BeerId aggregateId, Guid commitId, BeerName beerName,
	Quantity quantity) : Command(aggregateId, commitId)
{
	public readonly BeerId BeerId = aggregateId;
	public readonly BeerName BeerName = beerName;
	public readonly Quantity Quantity = quantity;
}