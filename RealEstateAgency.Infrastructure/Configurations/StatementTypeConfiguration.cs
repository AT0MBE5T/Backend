using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstateAgency.Core.Models;

namespace RealEstateAgency.Infrastructure.Configurations
{
    public class StatementTypeConfiguration : IEntityTypeConfiguration<StatementType>
    {
        public void Configure(EntityTypeBuilder<StatementType> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasMany(st => st.StatementsNavigation)
                    .WithOne(s => s.StatementTypeNavigation)
                    .HasForeignKey(s => s.StatementTypeId);
        }
    }
}
