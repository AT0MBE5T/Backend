using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstateAgency.Core.Models;

namespace RealEstateAgency.Infrastructure.Configurations;

public class ViewConfiguration:  IEntityTypeConfiguration<View>
{
    public void Configure(EntityTypeBuilder<View> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasOne(q => q.AnnouncementNavigation)
            .WithMany(u => u.ViewsNavigation)
            .HasForeignKey(q => q.AnnouncementId);

        builder.HasOne(q => q.UserNavigation)
            .WithMany(a => a.ViewsNavigation)
            .HasForeignKey(q => q.UserId);
    }
}