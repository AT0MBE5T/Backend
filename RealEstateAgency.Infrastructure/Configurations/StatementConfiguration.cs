using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstateAgency.Core.Models;

namespace RealEstateAgency.Infrastructure.Configurations
{
    public class StatementConfiguration : IEntityTypeConfiguration<Statement>
    {
        public void Configure(EntityTypeBuilder<Statement> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(s => s.StatementTypeNavigation)
                .WithMany(st => st.StatementsNavigation)
                .HasForeignKey(s => s.StatementTypeId);

            builder.HasOne(s => s.PropertyNavigation)
                .WithOne(p => p.StatementNavigation)
                .HasForeignKey<Statement>(s => s.PropertyId);

            builder.HasOne(s => s.UserNavigation)
                .WithMany(u => u.StatementsNavigation)
                .HasForeignKey(s => s.UserId);
        }
    }
}
