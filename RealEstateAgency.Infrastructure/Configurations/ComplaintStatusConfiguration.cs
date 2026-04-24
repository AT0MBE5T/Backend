using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstateAgency.Core.Entities;

namespace RealEstateAgency.Infrastructure.Configurations;

public class ComplaintStatusConfiguration:  IEntityTypeConfiguration<ComplaintStatus>
{
    public void Configure(EntityTypeBuilder<ComplaintStatus> builder)
    {
        builder.HasKey(e => e.Id);

        builder.HasMany(c => c.ComplaintsNavigation)
            .WithOne(u => u.ComplaintStatusNavigation)
            .HasForeignKey(c => c.StatusId);
    }
}