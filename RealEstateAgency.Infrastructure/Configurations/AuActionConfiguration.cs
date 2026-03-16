using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstateAgency.Core.Models;

namespace RealEstateAgency.Infrastructure.Configurations
{
    public class AuActionConfiguration : IEntityTypeConfiguration<AuAction>
    {
        public void Configure(EntityTypeBuilder<AuAction> builder)
        {
            builder.HasKey(x => x.Id);

            builder
                .HasMany(a => a.AuHistoriesNavigation)
                .WithOne(h => h.ActionNavigation)
                .HasForeignKey(h => h.ActionId);
        }
    }
}
