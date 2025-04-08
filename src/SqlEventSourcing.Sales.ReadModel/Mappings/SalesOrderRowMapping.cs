using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SqlEventSourcing.Sales.ReadModel.Dtos;

namespace SqlEventSourcing.Sales.ReadModel.Mappings;

public class SalesOrderRowMapping : IEntityTypeConfiguration<SalesOrderRow>
{
    public void Configure(EntityTypeBuilder<SalesOrderRow> builder)
    {
        builder.ToTable("SalesOrderRow");
        builder.HasKey(t => t.Id);

        builder.Property(t => t.BeerId).HasMaxLength(50);
        builder.Property(t => t.BeerName).HasMaxLength(100);
        builder.Property(t => t.Quantity).HasColumnName("Quantity");
        builder.Property(t => t.Quantity).HasColumnName("UnitOfMeasure").HasMaxLength(5);
        builder.Property(t => t.Price).HasColumnName("Price");
        builder.Property(t => t.Price).HasColumnName("Currency").HasMaxLength(3);
    }
}