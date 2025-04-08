using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Muflone.Persistence.Sql.Persistence;
using SqlEventSourcing.Sales.ReadModel.Dtos;
using SqlEventSourcing.Sales.ReadModel.Mappings;
using SqlEventSourcing.Shared.CustomTypes;
using Availability = SqlEventSourcing.Sales.ReadModel.Dtos.Availability;

namespace SqlEventSourcing.Sales.ReadModel.Facade;

public class SalesDbContext(SqlOptions sqlOptions) : DbContext
{
    public DbSet<SalesOrder> SalesOrders { get; set; }
    public DbSet<SalesOrderRow> SalesOrderRows { get; set; }
    public DbSet<Availability> Availabilities { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        optionsBuilder.UseSqlServer(sqlOptions.ConnectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ApplyConfiguration(new SalesOrderMapping());
        modelBuilder.ApplyConfiguration(new SalesOrderRowMapping());
        modelBuilder.ApplyConfiguration(new AvailabilityMapping());

        // modelBuilder.Entity<SalesOrder>()
        //     .HasMany(e => e.SalesOrderRows)
        //     .WithOne(e => e.SalesOrder)
        //     .HasForeignKey(e => e.OrderId)
        //     .IsRequired();
        //
        // modelBuilder.Entity<SalesOrderRow>()
        //     .HasOne(e => e.SalesOrder)
        //     .WithMany(e => e.SalesOrderRows)
        //     .HasForeignKey(e => e.OrderId)
        //     .IsRequired();
    }
}