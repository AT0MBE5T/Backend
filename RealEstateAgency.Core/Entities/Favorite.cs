using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealEstateAgency.Core.Entities;

[Table("t_favorite")]
public class Favorite
{
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