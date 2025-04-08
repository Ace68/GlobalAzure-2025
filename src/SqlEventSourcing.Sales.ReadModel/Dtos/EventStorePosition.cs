using SqlEventSourcing.Shared.Entities;
using SqlEventSourcing.Shared.ReadModel;

namespace SqlEventSourcing.Sales.ReadModel.Dtos;

public class EventStorePosition
{
    public int Id { get; private set; }
    public long CommitPosition { get; private set; }
    public long PreparePosition { get; private set; }
    
    protected EventStorePosition() { }

    public static EventStorePosition Create(long commitPosition, long preparePosition)
    {
        return new EventStorePosition(commitPosition, preparePosition);
    }

    private EventStorePosition(long commitPosition, long preparePosition)
    {
        Id = 1;
        CommitPosition = commitPosition;
        PreparePosition = preparePosition;
    }

    public void UpdatePositions(long commitPosition, long preparePosition)
    {
        CommitPosition = commitPosition;
        PreparePosition = preparePosition;
    }
}