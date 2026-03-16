using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealEstateAgency.Core.Models;

[Table("t_verification")]
public class Verification
{
        [Key]
        [Column("id")]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [Column("announcement_id")]
        public required Guid AnnouncementId { get; set; }

        [Required]
        [Column("created_at")]
        public required DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        [Required]
        [Column("created_by")]
        public required Guid CreatedBy { get; set; }

        public User? UserNavigation { get; set; }
        public Announcement? AnnouncementNavigation { get; set; }
}