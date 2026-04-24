using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstateAgency.Core.Entities;

namespace RealEstateAgency.Infrastructure.Configurations;

public class ChatTypeConfiguration: IEntityTypeConfiguration<ChatType>
{
    public void Configure(EntityTypeBuilder<ChatType> builder)
    {
        builder.HasKey(pt => pt.Id);

        builder.HasMany(pt => pt.ChatsNavigation)
            .WithOne(p => p.ChatTypeNavigation)
            .HasForeignKey(p => p.TypeId);
    }
}