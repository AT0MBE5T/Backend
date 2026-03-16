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
    public class UserTokenConfiguration : IEntityTypeConfiguration<IdentityUserToken<Guid>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserToken<Guid>> builder)
        {
            builder.Property(t => t.UserId).HasColumnName("user_id");
            builder.Property(t => t.LoginProvider).HasColumnName("login_provider");
            builder.Property(t => t.Name).HasColumnName("name");
            builder.Property(t => t.Value).HasColumnName("value");
        }
    }
}
