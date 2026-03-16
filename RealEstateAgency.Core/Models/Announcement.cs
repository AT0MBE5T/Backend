using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealEstateAgency.Core.Models
{
    [Table("t_announcement")]
    public class Announcement
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [Column("statement_id")]
        public Guid StatementId { get; set; }

        [Required]
        [Column("published_at")]
        public required DateTime PublishedAt { get; set; } = DateTime.UtcNow;

        [Column("closed_at")]
        public required DateTime? ClosedAt { get; set; }
        
        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }
        
        [Column("updated_by")]
        public Guid? UpdatedBy { get; set; }

        public Statement? StatementNavigation { get; set; }
        public ICollection<Comment> CommentsNavigation { get; set; } = [];
        public ICollection<Question> QuestionsNavigation { get; set; } = [];
        public Verification? VerificationNavigation { get; set; }
        public Payment? PaymentNavigation { get; set; }
        public User? UserNavigation { get; set; }
        public ICollection<View> ViewsNavigation { get; set; } = [];
        public ICollection<Complaint> ComplaintsNavigation { get; set; } = [];
        public ICollection<Favorite> FavoritesNavigation { get; set; } = [];
    }
}
