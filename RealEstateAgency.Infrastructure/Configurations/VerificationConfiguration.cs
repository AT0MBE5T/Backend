using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstateAgency.Core.Models;

namespace RealEstateAgency.Infrastructure.Configurations;

public class VerificationConfiguration : IEntityTypeConfiguration<Verification>
{
    public void Configure(EntityTypeBuilder<Verification> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasOne(q => q.AnnouncementNavigation)
            .WithOne(u => u.VerificationNavigation)
            .HasForeignKey<Verification>(q => q.AnnouncementId);

        builder.HasOne(q => q.UserNavigation)
            .WithMany(a => a.VerificationsNavigation)
            .HasForeignKey(q => q.CreatedBy);
    }
}