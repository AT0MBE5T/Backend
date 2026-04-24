using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstateAgency.Core.Entities;

namespace RealEstateAgency.Infrastructure.Configurations;

public class PaymentConfiguration: IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.HasKey(p => p.Id);
            
        builder
            .HasOne(p => p.CustomerNavigation)
            .WithMany(i => i.PaymentsNavigation)
            .HasForeignKey(p => p.CustomerId);

        builder
            .HasOne(p => p.AnnouncementNavigation)
            .WithOne(a => a.PaymentNavigation)
            .HasForeignKey<Payment>(p => p.AnnouncementId);
    }
}