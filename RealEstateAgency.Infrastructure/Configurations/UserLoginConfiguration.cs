using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstateAgency.Core.Models;

namespace RealEstateAgency.Infrastructure.Configurations
{
    internal class UserLoginConfiguration : IEntityTypeConfiguration<IdentityUserLogin<Guid>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserLogin<Guid>> builder)
        {
            builder.Property(l => l.UserId).HasColumnName("user_id");
            builder.Property(l => l.LoginProvider).HasColumnName("login_provider");
            builder.Property(l => l.ProviderKey).HasColumnName("provider_key");
            builder.Property(l => l.ProviderDisplayName).HasColumnName("provider_display_name");
        }
    }
}
