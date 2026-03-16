using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealEstateAgency.Core.Models
{
    [Table("t_au_history")]
    public class AuHistory
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [Column("action_id")]
        public required Guid ActionId { get; set; }

        [Required]
        [Column("user_id")]
        public required Guid UserId { get; set; }

        [Required]
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        [Column("details")]
        [StringLength(maximumLength: 256, MinimumLength = 5)]
        public required string Details { get; set; }

        public AuAction? ActionNavigation { get; set; }
        public User? UserNavigation { get; set; }
    }
}
