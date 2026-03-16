using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstateAgency.Core.Models;

namespace RealEstateAgency.Infrastructure.Configurations
{
    public class PropertyTypeConfiguration : IEntityTypeConfiguration<PropertyType>
    {
        public void Configure(EntityTypeBuilder<PropertyType> builder)
        {
            builder.HasKey(pt => pt.Id);

            builder.HasMany(pt => pt.PropertiesNavigation)
                .WithOne(p => p.PropertyTypeNavigation)
                .HasForeignKey(p => p.PropertyTypeId);
        }
    }
}
