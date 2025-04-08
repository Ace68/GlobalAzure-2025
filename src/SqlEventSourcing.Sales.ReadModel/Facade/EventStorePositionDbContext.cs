using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SqlEventSourcing.Sales.ReadModel.Mappings;
using EventStorePosition = SqlEventSourcing.Sales.ReadModel.Dtos.EventStorePosition;

namespace SqlEventSourcing.Sales.ReadModel.Facade;

public class EventStorePositionDbContext(string connectionString) : DbContext
{
    public DbSet<EventStorePosition> EventStorePositions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        optionsBuilder.UseSqlServer(new SqlConnection(connectionString));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ApplyConfiguration(new EventStorePositionMapping());
    }
}