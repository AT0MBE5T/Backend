using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstateAgency.Core.Entities;

namespace RealEstateAgency.Infrastructure.Configurations;

public class MessageConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.HasKey(pt => pt.Id);

        builder.HasOne(pt => pt.ChatNavigation)
            .WithMany(p => p.MessagesNavigation)
            .HasForeignKey(p => p.ChatId);
        
        builder.HasOne(pt => pt.UserNavigation)
            .WithMany(p => p.MessagesNavigation)
            .HasForeignKey(p => p.SenderId);
    }
}