using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configuration;

public class AvailabilityItemConfiguration : IEntityTypeConfiguration<AvailabilityItem>
{
    public void Configure(EntityTypeBuilder<AvailabilityItem> builder)
    {
        builder.ToTable(nameof(AvailabilityItem));
        builder.HasKey(e => e.Time);
    }
}