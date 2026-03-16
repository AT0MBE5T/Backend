using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealEstateAgency.Core.Models;

[Table("t_view")]
public class View
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; } =  Guid.NewGuid();
    
    [Required]
    [Column("user_id")]
    public required Guid UserId { get; set; }
    
    [Required]
    [Column("announcement_id")]
    public required Guid AnnouncementId { get; set; }
    
    [Column("created_at")]
    public DateTime CreatedAt { get; set; } =  DateTime.UtcNow;
    
    public User? UserNavigation { get; set; }
    public Announcement? AnnouncementNavigation { get; set; }
}