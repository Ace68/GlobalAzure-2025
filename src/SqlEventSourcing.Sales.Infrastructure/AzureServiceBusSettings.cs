namespace SqlEventSourcing.Sales.Infrastructure;

public class AzureServiceBusSettings
{
    public string ConnectionString { get; init; } = string.Empty;
    public string TopicName { get; init; } = string.Empty;
    public string ClientId { get; init; } = string.Empty;
}