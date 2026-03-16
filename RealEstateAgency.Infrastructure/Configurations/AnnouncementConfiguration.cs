using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstateAgency.Core.Models;

namespace RealEstateAgency.Infrastructure.Configurations
{
    public class AnnouncementConfiguration : IEntityTypeConfiguration<Announcement>
    {
        public void Configure(EntityTypeBuilder<Announcement> builder)
        {
            builder.HasKey(e => e.Id);

            builder
                .HasOne(a => a.StatementNavigation)
                .WithOne(s => s.AnnouncementNavigation)
                .HasForeignKey<Announcement>(a => a.StatementId);

            builder
                .HasMany(a => a.CommentsNavigation)
                .WithOne(c => c.AnnouncementNavigation)
                .HasForeignKey(c => c.AnnouncementId);

            builder
                .HasMany(a => a.QuestionsNavigation)
                .WithOne(c => c.AnnouncementNavigation)
                .HasForeignKey(c => c.AnnouncementId);
            
            builder
                .HasOne(a => a.UserNavigation)
                .WithMany(u => u.AnnouncementsNavigation)
                .HasForeignKey(c => c.UpdatedBy);
            
            builder
                .HasOne(a => a.VerificationNavigation)
                .WithOne(c => c.AnnouncementNavigation)
                .HasForeignKey<Verification>(c => c.AnnouncementId);
            
            builder
                .HasMany(a => a.ViewsNavigation)
                .WithOne(c => c.AnnouncementNavigation)
                .HasForeignKey(c => c.AnnouncementId);
            
            builder
                .HasMany(a => a.ComplaintsNavigation)
                .WithOne(c => c.AnnouncementNavigation)
                .HasForeignKey(c => c.AnnouncementId);
            
            builder
                .HasMany(a => a.FavoritesNavigation)
                .WithOne(c => c.AnnouncementNavigation)
                .HasForeignKey(c => c.AnnouncementId);
        }
    }
}
