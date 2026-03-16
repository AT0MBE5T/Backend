using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstateAgency.Core.Models;

namespace RealEstateAgency.Infrastructure.Configurations;

public class ComplaintConfiguration:  IEntityTypeConfiguration<Complaint>
{
    public void Configure(EntityTypeBuilder<Complaint> builder)
    {
        builder.HasKey(e => e.Id);

        builder.HasOne(c => c.ComplaintStatusNavigation)
            .WithMany(u => u.ComplaintsNavigation)
            .HasForeignKey(c => c.StatusId);
        
        builder.HasOne(c => c.ComplaintTypeNavigation)
            .WithMany(u => u.ComplaintsNavigation)
            .HasForeignKey(c => c.TypeId);
        
        builder
            .HasOne(c => c.UserNavigation)
            .WithMany(u => u.ComplaintsNavigation)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder
            .HasOne(c => c.AdminNavigation)
            .WithMany()
            .HasForeignKey(c => c.AdminId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(c => c.AnnouncementNavigation)
            .WithMany(u => u.ComplaintsNavigation)
            .HasForeignKey(c => c.AnnouncementId);
    }
}