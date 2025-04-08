using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SqlEventSourcing.Sales.ReadModel.Dtos;

namespace SqlEventSourcing.Sales.ReadModel.Mappings;

public class AvailabilityMapping : IEntityTypeConfiguration<Availability>
{
    public void Configure(EntityTypeBuilder<Availability> builder)
    {
        builder.ToTable("Availability");
        builder.HasKey(t => t.Id);
        
        builder.Property(t => t.BeerId).HasMaxLength(50);
        builder.Property(t => t.BeerName).HasMaxLength(100);
    }
}