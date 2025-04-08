using Microsoft.Extensions.Logging;
using Muflone.Messages.Events;
using Muflone.Transport.Azure.Consumers;
using Muflone.Transport.Azure.Models;
using SqlEventSourcing.Sales.ReadModel.EventHandlers;
using SqlEventSourcing.Sales.ReadModel.Services;
using SqlEventSourcing.Sales.SharedKernel.Events;

namespace SqlEventSourcing.Sales.Infrastructure.Events;

public sealed class SalesOrderDeliveryDateSetConsumer(
    ISalesOrderService salesOrderService,
    AzureServiceBusConfiguration azureServiceBusConfiguration,
    ILoggerFactory loggerFactory)
    : DomainEventConsumerBase<SalesOrderDeliveryDateSet>(azureServiceBusConfiguration, loggerFactory)
{
    protected override IEnumerable<IDomainEventHandlerAsync<SalesOrderDeliveryDateSet>> HandlersAsync { get; } = new List<DomainEventHandlerAsync<SalesOrderDeliveryDateSet>>
    {
        new SalesOrderDeliveryDateSetEventHandler(loggerFactory, salesOrderService)
    };
}