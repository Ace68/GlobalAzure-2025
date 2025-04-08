using Microsoft.Extensions.Logging;
using Muflone.Messages.Commands;
using Muflone.Persistence;
using Muflone.Transport.Azure.Consumers;
using Muflone.Transport.Azure.Models;
using SqlEventSourcing.Sales.Domain.CommandHandlers;
using SqlEventSourcing.Sales.SharedKernel.Commands;

namespace SqlEventSourcing.Sales.Infrastructure.Commands;

public sealed class CreateSalesOrderConsumer(
    IRepository repository,
    AzureServiceBusConfiguration azureServiceBusConfiguration,
    ILoggerFactory loggerFactory)
    : CommandConsumerBase<CreateSalesOrder>(azureServiceBusConfiguration, loggerFactory)
{
    protected override ICommandHandlerAsync<CreateSalesOrder> HandlerAsync { get; } =
        new CreateSalesOrderCommandHandler(repository, loggerFactory);
}