using Muflone.Persistence.Sql.Persistence;

namespace Muflone.Persistence.Sql.Dispatcher;

public interface IEventStorePositionRepository
{
    Task<IEventStorePosition> GetLastPosition();
    Task Save(IEventStorePosition position);
}