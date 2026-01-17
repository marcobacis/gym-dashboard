using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configuration;

public class AvailabilityItemConfiguration : IEntityTypeConfiguration<AvailabilityItem>
{
    public void Configure(EntityTypeBuilder<AvailabilityItem> builder)
    {
        builder.ToTable(nameof(AvailabilityItem));
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasDefaultValueSql("uuidv7()");
        
        builder.HasOne(e => e.Gym).WithMany().HasForeignKey(i => i.GymId);
    }
}