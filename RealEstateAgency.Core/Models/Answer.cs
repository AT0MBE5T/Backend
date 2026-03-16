using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealEstateAgency.Core.Models
{
    [Table("t_answer")]
    public class Answer
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [Column("text")]
        [StringLength(maximumLength: 256, MinimumLength = 5)]
        public required string Text { get; set; }

        [Required]
        [Column("question_id")]
        public required Guid QuestionId { get; set; }

        [Required]
        [Column("user_id")]
        public required Guid UserId { get; set; }

        [Required]
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Question? QuestionNavigation { get; set; }
        public User? UserNavigation { get; set; }
    }
}
