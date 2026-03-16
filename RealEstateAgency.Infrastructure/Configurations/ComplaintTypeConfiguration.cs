using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstateAgency.Core.Models;

namespace RealEstateAgency.Infrastructure.Configurations;

public class ComplaintTypeConfiguration: IEntityTypeConfiguration<ComplaintType>
{
    public void Configure(EntityTypeBuilder<ComplaintType> builder)
    {
        builder.HasKey(e => e.Id);

        builder.HasMany(c => c.ComplaintsNavigation)
            .WithOne(u => u.ComplaintTypeNavigation)
            .HasForeignKey(c => c.TypeId);
    }
}