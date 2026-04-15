using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstateAgency.Core.Models;

namespace RealEstateAgency.Infrastructure.Configurations;

public class UserPushSubscriptionConfiguration : IEntityTypeConfiguration<UserPushSubscription>
{
    public void Configure(EntityTypeBuilder<UserPushSubscription> builder)
    {
        builder.HasKey(pt => pt.Id);

        builder.HasOne(pt => pt.UserNavigation)
            .WithMany(p => p.UserPushSubscriptionsNavigation)
            .HasForeignKey(p => p.UserId);
    }
}