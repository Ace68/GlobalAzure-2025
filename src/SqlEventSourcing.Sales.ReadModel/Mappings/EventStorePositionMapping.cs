using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SqlEventSourcing.Sales.ReadModel.Dtos;

namespace SqlEventSourcing.Sales.ReadModel.Mappings;

public class EventStorePositionMapping : IEntityTypeConfiguration<EventStorePosition>
{
    public void Configure(EntityTypeBuilder<EventStorePosition> builder)
    {
        builder.ToTable("EventStorePosition");
        builder.HasKey(t => t.Id);
    }
}