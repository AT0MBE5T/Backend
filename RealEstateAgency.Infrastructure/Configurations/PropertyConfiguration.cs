using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstateAgency.Core.Models;

namespace RealEstateAgency.Infrastructure.Configurations
{
    public class PropertyConfiguration : IEntityTypeConfiguration<Property>
    {
        public void Configure(EntityTypeBuilder<Property> builder)
        {
            builder.HasKey(p => p.Id);

            builder
                .HasOne(p => p.PropertyTypeNavigation)
                .WithMany(pt => pt.PropertiesNavigation)
                .HasForeignKey(p => p.PropertyTypeId);
        }
    }
}
