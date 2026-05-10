using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstateAgency.Core.Entities;

namespace RealEstateAgency.Infrastructure.Configurations;

public class SupportConfiguration:  IEntityTypeConfiguration<Support>
{
    public void Configure(EntityTypeBuilder<Support> builder)
    {
        builder.HasKey(e => e.Id);
        
        builder
            .HasOne(c => c.UserNavigation)
            .WithMany(u => u.SupportsNavigation)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder
            .HasOne(c => c.AdminNavigation)
            .WithMany()
            .HasForeignKey(c => c.AdminId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}