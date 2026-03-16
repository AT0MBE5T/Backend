using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstateAgency.Core.Models;

namespace RealEstateAgency.Infrastructure.Configurations;

public class ImageConfiguration: IEntityTypeConfiguration<Image>
{
    public void Configure(EntityTypeBuilder<Image> builder)
    {
        builder.HasKey(i => i.Id);
        
        builder
            .HasOne(i => i.Property)
            .WithMany(p => p.ImagesNavigation)
            .HasForeignKey(p => p.PropertyId);
    }
}