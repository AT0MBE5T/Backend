using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstateAgency.Core.Entities;

namespace RealEstateAgency.Infrastructure.Configurations;

public class ChatConfiguration : IEntityTypeConfiguration<Chat>
{
    public void Configure(EntityTypeBuilder<Chat> builder)
    {
        builder.HasKey(e => e.Id);

        builder.HasOne(c => c.ChatTypeNavigation)
            .WithMany(u => u.ChatsNavigation)
            .HasForeignKey(c => c.TypeId);
        
        builder.HasOne(c => c.AnnouncementNavigation)
            .WithMany(u => u.ChatsNavigation)
            .HasForeignKey(c => c.AnnouncementId);
    }
}