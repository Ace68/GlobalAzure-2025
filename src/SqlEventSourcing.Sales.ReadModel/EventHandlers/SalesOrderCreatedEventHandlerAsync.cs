﻿using Microsoft.Extensions.Logging;
using Muflone.Messages.Events;
using SqlEventSourcing.Sales.ReadModel.Services;
using SqlEventSourcing.Sales.SharedKernel.Events;

namespace SqlEventSourcing.Sales.ReadModel.EventHandlers;

public sealed class SalesOrderCreatedEventHandlerAsync(ILoggerFactory loggerFactory, ISalesOrderService salesOrderService)
	: DomainEventHandlerAsync<SalesOrderCreated>(loggerFactory)
{
	public override async Task HandleAsync(SalesOrderCreated @event, CancellationToken cancellationToken = new())
	{
		try
		{
			await salesOrderService.CreateSalesOrderAsync(@event.SalesOrderId, @event.SalesOrderNumber, @event.CustomerId,
				@event.CustomerName, @event.OrderDate, @event.Rows, cancellationToken);
		}
		catch (Exception ex)
		{
			Logger.LogError(ex, "Error handling sales order created event");
			throw;
		}
	}
}