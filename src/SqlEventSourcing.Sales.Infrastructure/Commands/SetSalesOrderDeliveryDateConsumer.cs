using Microsoft.Extensions.Logging;
using Muflone.Messages.Commands;
using Muflone.Persistence;
using Muflone.Transport.Azure.Consumers;
using Muflone.Transport.Azure.Models;
using SqlEventSourcing.Sales.Domain.CommandHandlers;
using SqlEventSourcing.Sales.SharedKernel.Commands;

namespace SqlEventSourcing.Sales.Infrastructure.Commands;

public sealed class SetSalesOrderDeliveryDateConsumer(
    IRepository repository,
    AzureServiceBusConfiguration azureServiceBusConfiguration,
    ILoggerFactory loggerFactory)
    : CommandConsumerBase<SetSalesOrderDeliveryDate>(azureServiceBusConfiguration, loggerFactory)
{
    protected override ICommandHandlerAsync<SetSalesOrderDeliveryDate> HandlerAsync { get; } =
        new SetSalesOrderDeliveryDateCommandHandler(repository, loggerFactory);
}