namespace GlobalAzure.Services;

public interface IAggregateService
{
    Task<IEnumerable<ResolvedEvent>?> GetAggregateStreamByIdAsync(string aggregateId, int aggregateVersion,
        CancellationToken cancellationToken);
    
    Task<IEnumerable<ResolvedAggregate>?> GetAggregatesAsync(CancellationToken cancellationToken);
}