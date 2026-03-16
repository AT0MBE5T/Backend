using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstateAgency.Core.Models;

namespace RealEstateAgency.Infrastructure.Configurations
{
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.HasKey(e => e.Id);

            builder.HasOne(c => c.UserNavigation)
                .WithMany(u => u.CommentsNavigation)
                .HasForeignKey(c => c.UserId);

            builder.HasOne(c => c.AnnouncementNavigation)
                .WithMany(u => u.CommentsNavigation)
                .HasForeignKey(c => c.AnnouncementId);
        }
    }
}
