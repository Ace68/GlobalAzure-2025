using Microsoft.Extensions.Logging;
using Muflone.Messages.Events;
using SqlEventSourcing.Sales.ReadModel.Services;
using SqlEventSourcing.Sales.SharedKernel.Events;

namespace SqlEventSourcing.Sales.ReadModel.EventHandlers;

public sealed class AvailabilityUpdatedDueToWarehousesNotificationEventHandler(ILoggerFactory loggerFactory, IAvailabilityService availabilityService) : DomainEventHandlerAsync<AvailabilityUpdatedDueToWarehousesNotification>(loggerFactory)
{
	public override async Task HandleAsync(AvailabilityUpdatedDueToWarehousesNotification @event,
		CancellationToken cancellationToken = new())
	{
		cancellationToken.ThrowIfCancellationRequested();

		await availabilityService.UpdateAvailabilityAsync(@event.BeerId, @event.BeerName, @event.Quantity, cancellationToken);
	}
}