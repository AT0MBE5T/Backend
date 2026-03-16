using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstateAgency.Core.Models;

namespace RealEstateAgency.Infrastructure.Configurations
{
    public class AnswerConfiguration : IEntityTypeConfiguration<Answer>
    {
        public void Configure(EntityTypeBuilder<Answer> builder)
        {
            builder.HasKey(e => e.Id);

            builder
                .HasOne(a => a.QuestionNavigation)
                .WithOne(q => q.AnswerNavigation)
                .HasForeignKey<Answer>(a => a.QuestionId);

            builder
                .HasOne(a => a.UserNavigation)
                .WithMany(u => u.AnswersNavigation)
                .HasForeignKey(a => a.UserId);
        }
    }
}
