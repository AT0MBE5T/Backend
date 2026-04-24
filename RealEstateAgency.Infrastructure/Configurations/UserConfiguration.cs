using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstateAgency.Core.Entities;

namespace RealEstateAgency.Infrastructure.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(u => u.Id).HasColumnName("id");
            builder.Property(u => u.UserName).HasColumnName("login");
            builder.Property(u => u.NormalizedUserName).HasColumnName("login_normalized");
            builder.Property(u => u.Email).HasColumnName("email");
            builder.Property(u => u.NormalizedEmail).HasColumnName("email_normalized");
            builder.Property(u => u.EmailConfirmed).HasColumnName("email_confirmed");
            builder.Property(u => u.PhoneNumber).HasColumnName("phone_number");
            builder.Property(u => u.PhoneNumberConfirmed).HasColumnName("phone_number_confirmed");
            builder.Property(u => u.PasswordHash).HasColumnName("password_hash");
            builder.Property(u => u.SecurityStamp).HasColumnName("security_stamp");
            builder.Property(u => u.ConcurrencyStamp).HasColumnName("concurrency_stamp");
            builder.Property(u => u.TwoFactorEnabled).HasColumnName("two_factor_enabled");
            builder.Property(u => u.LockoutEnd).HasColumnName("lockout_end");
            builder.Property(u => u.LockoutEnabled).HasColumnName("lockout_enabled");
            builder.Property(u => u.AccessFailedCount).HasColumnName("access_failed_count");
            builder.Property(u => u.Avatar).HasColumnName("avatar");
            builder.Property(u => u.PublicAvatarId).HasColumnName("public_avatar_id");

            builder.HasMany(u => u.StatementsNavigation)
                .WithOne(s => s.UserNavigation)
                .HasForeignKey(s => s.UserId);

            builder.HasMany(u => u.CommentsNavigation)
                .WithOne(c => c.UserNavigation)
                .HasForeignKey(c => c.UserId);

            builder.HasMany(u => u.QuestionsNavigation)
                .WithOne(q => q.UserNavigation)
                .HasForeignKey(q => q.UserId);

            builder.HasMany(u => u.AnswersNavigation)
                .WithOne(a => a.UserNavigation)
                .HasForeignKey(a => a.UserId);

            builder.HasMany(u => u.AuHistoriesNavigation)
                .WithOne(ah => ah.UserNavigation)
                .HasForeignKey(ah => ah.UserId);
            
            builder.HasMany(u => u.VerificationsNavigation)
                .WithOne(v => v.UserNavigation)
                .HasForeignKey(v => v.CreatedBy);
            
            builder.HasMany(u => u.FavoritesNavigation)
                .WithOne(a => a.UserNavigation)
                .HasForeignKey(a => a.UserId);

            builder.HasMany(u => u.ComplaintsNavigation)
                .WithOne(ah => ah.UserNavigation)
                .HasForeignKey(ah => ah.UserId);
            
            builder.HasMany(u => u.ViewsNavigation)
                .WithOne(v => v.UserNavigation)
                .HasForeignKey(v => v.UserId);
        }
    }
}
