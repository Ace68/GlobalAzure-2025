using Muflone.Messages.Events;
using SqlEventSourcing.Shared.CustomTypes;
using SqlEventSourcing.Shared.DomainIds;

namespace SqlEventSourcing.Shared.Messages.Sagas;

public sealed class BeerAvailabilityCommunicated(BeerId aggregateId, Guid correlationId, Availability availability)
	: IntegrationEvent(aggregateId, correlationId)
{
	public readonly BeerId BeerId = aggregateId;
	public readonly Availability Availability = availability;
}