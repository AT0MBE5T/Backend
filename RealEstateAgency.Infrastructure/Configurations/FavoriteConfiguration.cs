using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstateAgency.Core.Entities;

namespace RealEstateAgency.Infrastructure.Configurations;

public class FavoriteConfiguration:  IEntityTypeConfiguration<Favorite>
{
    public void Configure(EntityTypeBuilder<Favorite> builder)
    {
        builder.HasKey(x=> new
        {
            x.UserId, x.AnnouncementId
        });

        builder.HasOne(q => q.AnnouncementNavigation)
            .WithMany(u => u.FavoritesNavigation)
            .HasForeignKey(q => q.AnnouncementId);

        builder.HasOne(q => q.UserNavigation)
            .WithMany(a => a.FavoritesNavigation)
            .HasForeignKey(q => q.UserId);
    }
}