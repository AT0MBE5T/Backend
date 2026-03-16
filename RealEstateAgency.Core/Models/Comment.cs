using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealEstateAgency.Core.Models
{
    [Table("t_comment")]
    public class Comment
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [Column("text")]
        [StringLength(maximumLength: 256, MinimumLength = 5)]
        public required string Text { get; set; }

        [Required]
        [Column("user_id")]
        public required Guid UserId { get; set; }

        [Required]
        [Column("announcement_id")]
        public required Guid AnnouncementId { get; set; }

        [Required]
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public User? UserNavigation { get; set; }
        public Announcement? AnnouncementNavigation { get; set; }
    }
}
