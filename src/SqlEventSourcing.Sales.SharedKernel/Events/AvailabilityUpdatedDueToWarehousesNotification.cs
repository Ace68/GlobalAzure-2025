using Muflone.Messages.Events;
using SqlEventSourcing.Shared.CustomTypes;
using SqlEventSourcing.Shared.DomainIds;

namespace SqlEventSourcing.Sales.SharedKernel.Events;

public class AvailabilityUpdatedDueToWarehousesNotification(BeerId aggregateId, Guid commitId, BeerName beerName,
	Quantity quantity) : DomainEvent(aggregateId, commitId)
{
	public readonly BeerId BeerId = aggregateId;
	public readonly BeerName BeerName = beerName;
	public readonly Quantity Quantity = quantity;
}