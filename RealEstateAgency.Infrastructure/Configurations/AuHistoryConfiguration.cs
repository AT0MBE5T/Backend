using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstateAgency.Core.Models;

namespace RealEstateAgency.Infrastructure.Configurations
{
    public class AuHistoryConfiguration : IEntityTypeConfiguration<AuHistory>
    {
        public void Configure(EntityTypeBuilder<AuHistory> builder)
        {
            builder.HasKey(x => x.Id);

            builder
                .HasOne(ah => ah.UserNavigation)
                .WithMany(u => u.AuHistoriesNavigation)
                .HasForeignKey(ah => ah.UserId);

            builder
                .HasOne(ah => ah.ActionNavigation)
                .WithMany(aa => aa.AuHistoriesNavigation)
                .HasForeignKey(ah => ah.ActionId);
        }
    }
}
