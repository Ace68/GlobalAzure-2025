using System.Text;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Muflone.Messages.Events;
using Muflone.Persistence.Sql.Persistence;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Muflone.Persistence.Sql.Dispatcher;

public class EventDispatcher : IHostedService
{
    private readonly IEventBus _eventBus;
    private readonly IEventStorePositionRepository _eventStorePositionRepository;
    private readonly SqlOptions _sqlOptions;
    private readonly ILogger _logger;
    private Position _lastProcessed;
    
    private Timer _timer;
    
    public EventDispatcher(ILoggerFactory loggerFactory, SqlOptions sqlOptions, IEventBus eventBus, IEventStorePositionRepository eventStorePositionRepository)
    {
        _logger = loggerFactory.CreateLogger(GetType()) ?? throw new ArgumentNullException(nameof(loggerFactory));
        _sqlOptions = sqlOptions ?? throw new ArgumentNullException(nameof(sqlOptions));
        _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        _eventStorePositionRepository = eventStorePositionRepository ?? throw new ArgumentNullException(nameof(eventStorePositionRepository));
        _lastProcessed = new Position(0, 0);
    }
    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
            cancellationToken.ThrowIfCancellationRequested();
        _logger.LogInformation("EventDispatcher started");

        var position = await _eventStorePositionRepository.GetLastPosition();
        _lastProcessed = new Position(position.CommitPosition, position.PreparePosition);
        
        _timer = new Timer(TimerCallback, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
            return Task.FromCanceled(cancellationToken);

        _logger.LogInformation("EventDispatcher stopped");

        return Task.CompletedTask;
    }

    private void TimerCallback(object state)
    {
        _timer.Change(Timeout.Infinite, 0); // Stop the timer
        
        try
        {
            _logger.LogInformation("Read events at: " + DateTime.UtcNow);
            
            Task.Run(async () => 
            {
                try
                {
                    await using var facade = new EventStoreFacade(_sqlOptions.ConnectionString);
                    var readResult = facade.EventStore
                        .Where(e => e.CommitPosition > _lastProcessed.CommitPosition)
                        .ToList();

                    foreach (var @event in readResult)
                    {
                        await PublishEvent(@event);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }

            });
        }
        finally
        {
            _timer.Change(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1)); // Restart the timer
        }
    }
    
    private async Task PublishEvent(EventRecord resolvedEvent)
    {
        if (!(resolvedEvent.CommitPosition > _lastProcessed.CommitPosition))
            return;

        var processedEvent = ProcessRawEvent(resolvedEvent);
        if (processedEvent != null)
        {
            processedEvent.Headers.Set(Constants.CommitPosition, resolvedEvent.CommitPosition.ToString());
            processedEvent.Headers.Set(Constants.PreparePosition, resolvedEvent.CommitPosition.ToString());
            
            await _eventBus.PublishAsync(processedEvent);
        }

        _lastProcessed = new Position(resolvedEvent.CommitPosition, resolvedEvent.CommitPosition);
        await _eventStorePositionRepository.Save(EventStorePosition.Create(_lastProcessed.CommitPosition, _lastProcessed.PreparePosition));
    }
    
    private DomainEvent? ProcessRawEvent(EventRecord rawEvent)
    {
        if (rawEvent.Metadata.Length > 0 && rawEvent.Data.Length > 0)
            return DeserializeEvent(rawEvent.Metadata, rawEvent.Data);

        return null;
    }
    
    private DomainEvent? DeserializeEvent(ReadOnlyMemory<byte> metadata, ReadOnlyMemory<byte> data)
    {
        if (JObject.Parse(Encoding.UTF8.GetString(metadata.ToArray())).Property("EventClrTypeName") == null)
            return null;
        var eventClrTypeName = JObject.Parse(Encoding.UTF8.GetString(metadata.ToArray())).Property("EventClrTypeName")!.Value;
        try
        {
            return (DomainEvent?)JsonConvert.DeserializeObject(Encoding.UTF8.GetString(data.ToArray()), Type.GetType((string)eventClrTypeName!)!);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return null;
        }
    }
}