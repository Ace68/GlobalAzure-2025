using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Muflone.Persistence.Sql.Dispatcher;
using Muflone.Persistence.Sql.Persistence;
using SqlEventSourcing.Sales.ReadModel.Facade;

namespace SqlEventSourcing.Sales.ReadModel.Services;

public sealed class EventStorePositionRepository : IEventStorePositionRepository
{
    private readonly SalesReadModelConfiguration _repositoryConfiguration;
    private readonly ILogger _logger;

    public EventStorePositionRepository(SalesReadModelConfiguration repositoryConfiguration,
        ILoggerFactory loggerFactory)
    {
        _repositoryConfiguration = repositoryConfiguration;
        _logger = loggerFactory.CreateLogger(GetType()) ?? throw new ArgumentNullException(nameof(loggerFactory));
    }
    
    public async Task<IEventStorePosition> GetLastPosition()
    {
        try
        {
            await using var dbContext = new EventStorePositionDbContext(_repositoryConfiguration.ConnectionString);
            var position = dbContext.EventStorePositions
                .OrderBy(p => p.CommitPosition)
                .AsQueryable();

            return !position.Any()
                ? EventStorePosition.Create(0, 0)
                : EventStorePosition.Create(position.Last().CommitPosition, position.Last().PreparePosition);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error reading EventStore position");
            throw;
        }
    }

    public async Task Save(IEventStorePosition position)
    {
        try
        {
            await using var dbContext = new EventStorePositionDbContext(_repositoryConfiguration.ConnectionString);
            var sqlPosition = dbContext.EventStorePositions
                .AsNoTracking()
                .Where(p => p.Id == 1)
                .AsQueryable();
            
            if (sqlPosition.Any())
            {
                var evnPosition = sqlPosition.First();
                evnPosition.UpdatePositions(position.CommitPosition, position.PreparePosition);
            }
            else
            {
                var newPosition = Dtos.EventStorePosition.Create(position.CommitPosition, position.PreparePosition);
                dbContext.EventStorePositions.Add(newPosition);                
            }
            await dbContext.SaveChangesAsync(CancellationToken.None);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating positions");
            throw;
        }
    }
}