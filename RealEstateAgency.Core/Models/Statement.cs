using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealEstateAgency.Core.Models
{
    [Table("t_statement")]
    public class Statement
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [Column("title")]
        [StringLength(maximumLength: 100, MinimumLength = 5)]
        public required string Title { get; set; }

        [Required]
        [Column("content")]
        [StringLength(maximumLength: 256, MinimumLength = 5)]
        public required string Content { get; set; }

        [Required]
        [Column("statement_type_id")]
        public required Guid StatementTypeId { get; set; }

        [Required]
        [Column("property_id")]
        public required Guid PropertyId { get; set; }

        [Required]
        [Column("user_id")]
        public required Guid UserId { get; set; }

        [Required]
        [Column("price")]
        [Range(1, double.MaxValue)]
        public required decimal Price { get; set; }

        [Required]
        [Column("created_at")]
        public required DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public StatementType? StatementTypeNavigation { get; set; }
        public Property? PropertyNavigation { get; set; }
        public User? UserNavigation { get; set; }
        public Announcement? AnnouncementNavigation { get; set; }
    }
}
