using Microsoft.Extensions.Logging;
using Muflone.Messages.Commands;
using Muflone.Persistence;
using SqlEventSourcing.Sales.Domain.Entities;
using SqlEventSourcing.Sales.SharedKernel.Commands;

namespace SqlEventSourcing.Sales.Domain.CommandHandlers;

public sealed class SetSalesOrderDeliveryDateCommandHandler(IRepository repository, ILoggerFactory loggerFactory)
    : CommandHandlerAsync<SetSalesOrderDeliveryDate>(repository, loggerFactory)
{
    public override async Task HandleAsync(SetSalesOrderDeliveryDate command, CancellationToken cancellationToken = new CancellationToken())
    {
        var aggregate = await Repository.GetByIdAsync<SalesOrder>(command.SalesOrderId, cancellationToken);
        aggregate!.SetDeliveryDate(command.DeliveryDate, command.MessageId);
        await Repository.SaveAsync(aggregate, Guid.NewGuid(), cancellationToken);
    }
}