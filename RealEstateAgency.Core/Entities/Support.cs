using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealEstateAgency.Core.Entities;

[Table("t_support")]
public class Support
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [Column("user_id")]
    public required Guid UserId { get; set; }

    [Required]
    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    [Required]
    [Column("user_note")]
    public string UserNote { get; set; } = string.Empty;
    
    [Column("admin_id")]
    public Guid? AdminId { get; set; }
    
    [Column("closed_at")]
    public DateTime? ClosedAt { get; set; }
    
    public User? UserNavigation { get; set; }
    public User? AdminNavigation { get; set; }
}