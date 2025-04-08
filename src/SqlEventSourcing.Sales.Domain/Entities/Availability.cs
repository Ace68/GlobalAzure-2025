using Muflone.Core;
using SqlEventSourcing.Sales.SharedKernel.Events;
using SqlEventSourcing.Shared.CustomTypes;
using SqlEventSourcing.Shared.DomainIds;

namespace SqlEventSourcing.Sales.Domain.Entities;

public class Availability : AggregateRoot
{
	internal BeerId _beerId;
	internal BeerName _beerName;
	internal Quantity _quantity;

	protected Availability()
	{
	}

	internal static Availability CreateAvailability(BeerId beerId, BeerName beerName, Quantity quantity, Guid correlationId)
	{
		return new Availability(beerId, beerName, quantity, correlationId);
	}

	private Availability(BeerId beerId, BeerName beerName, Quantity quantity, Guid correlationId)
	{
		RaiseEvent(new AvailabilityUpdatedDueToWarehousesNotification(beerId, correlationId, beerName, quantity));
	}

	private void Apply(AvailabilityUpdatedDueToWarehousesNotification @event)
	{
		Id = @event.BeerId;

		_beerId = @event.BeerId;
		_beerName = @event.BeerName;
		_quantity = @event.Quantity;
	}
}