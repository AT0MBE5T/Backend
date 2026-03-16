using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstateAgency.Core.Models;

namespace RealEstateAgency.Infrastructure.Configurations;

public class ChatMemberConfiguration : IEntityTypeConfiguration<ChatMember>
{
    public void Configure(EntityTypeBuilder<ChatMember> builder)
    {
        builder.HasKey(x=> new
        {
            x.ChatId, x.UserId
        });
        
        builder.HasOne(pt => pt.ChatNavigation)
            .WithMany(p => p.ChatMembersNavigation)
            .HasForeignKey(p => p.ChatId);
        
        builder.HasOne(pt => pt.UserNavigation)
            .WithMany(u => u.ChatMembersNavigation)
            .HasForeignKey(p => p.UserId);
    }
}