using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SqlEventSourcing.Sales.ReadModel.Dtos;

namespace SqlEventSourcing.Sales.ReadModel.Mappings;

public class SalesOrderMapping : IEntityTypeConfiguration<SalesOrder>
{
    public void Configure(EntityTypeBuilder<SalesOrder> builder)
    {
        builder.ToTable("SalesOrder");
        builder.HasKey(t => t.Id);

        builder.Property(t => t.OrderNumber).HasMaxLength(30);
        builder.Property(t => t.CustomerId).HasMaxLength(50);
        builder.Property(t => t.CustomerName).HasMaxLength(100);
        builder.Property(t => t.Status).HasMaxLength(30);
    }
}