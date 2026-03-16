using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealEstateAgency.Core.Models;

[Table("t_complaint")]
public class Complaint
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [Column("user_id")]
    public required Guid UserId { get; set; }
    
    [Required]
    [Column("announcement_id")]
    public required Guid AnnouncementId { get; set; }

    [Required]
    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    [Required]
    [Column("user_note")]
    public string UserNote { get; set; } = string.Empty;
    
    [Column("admin_id")]
    public Guid? AdminId { get; set; }
    
    [Column("admin_note")]
    public string? AdminNote { get; set; }
    
    [Column("processed_at")]
    public DateTime? ProcessedAt { get; set; }
    
    [Required]
    [Column("type_id")]
    public required Guid TypeId { get; set; }
    
    [Required]
    [Column("status_id")]
    public required Guid StatusId { get; set; }
    
    public User? UserNavigation { get; set; }
    public User? AdminNavigation { get; set; }
    public ComplaintStatus? ComplaintStatusNavigation { get; set; }
    public ComplaintType? ComplaintTypeNavigation { get; set; }
    public Announcement? AnnouncementNavigation { get; set; }
}