using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstateAgency.Core.Models;

namespace RealEstateAgency.Infrastructure.Configurations
{
    public class QuestionConfiguration : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(q => q.UserNavigation)
                .WithMany(u => u.QuestionsNavigation)
                .HasForeignKey(q => q.UserId);

            builder.HasOne(q => q.AnnouncementNavigation)
                .WithMany(a => a.QuestionsNavigation)
                .HasForeignKey(q => q.AnnouncementId);
        }
    }
}
