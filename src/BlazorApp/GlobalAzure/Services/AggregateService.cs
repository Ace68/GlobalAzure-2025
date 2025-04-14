using Newtonsoft.Json;

namespace GlobalAzure.Services;

internal sealed class AggregateService(HttpClient client) : IAggregateService
{
    public async Task<IEnumerable<ResolvedEvent>?> GetAggregateStreamByIdAsync(string aggregateId, int aggregateVersion,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        if (string.IsNullOrWhiteSpace(aggregateId))
            throw new ArgumentException("AggregateId is required", nameof(aggregateId));
        
        using var request =
            new HttpRequestMessage(HttpMethod.Get, $"{client.BaseAddress}v1/muflone/aggregate/{aggregateId}");

        var response = await client.SendAsync(request, cancellationToken);

        var content = await response.Content.ReadAsStringAsync(cancellationToken);

        return response.IsSuccessStatusCode 
            ? JsonConvert.DeserializeObject<IEnumerable<ResolvedEvent>>(content) 
            : [];
    }

    public async Task<IEnumerable<ResolvedAggregate>?> GetAggregatesAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        using var request =
            new HttpRequestMessage(HttpMethod.Get, $"{client.BaseAddress}v1/muflone/aggregate");

        var response = await client.SendAsync(request, cancellationToken);

        var content = await response.Content.ReadAsStringAsync(cancellationToken);

        return response.IsSuccessStatusCode 
            ? JsonConvert.DeserializeObject<IEnumerable<ResolvedAggregate>>(content) 
            : [];
    }
}