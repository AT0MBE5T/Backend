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
    public class RoleClaimConfiguration : IEntityTypeConfiguration<IdentityRoleClaim<Guid>>
    {
        public void Configure(EntityTypeBuilder<IdentityRoleClaim<Guid>> builder)
        {
            builder.Property(rc => rc.Id).HasColumnName("id");
            builder.Property(rc => rc.RoleId).HasColumnName("role_id");
            builder.Property(rc => rc.ClaimType).HasColumnName("claim_type");
            builder.Property(rc => rc.ClaimValue).HasColumnName("claim_value");
        }
    }
}
