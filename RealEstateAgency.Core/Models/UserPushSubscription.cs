using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealEstateAgency.Core.Models;

[Table("t_user_push_subscription")]
public class UserPushSubscription
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; } = Guid.NewGuid();
    
    [Required]
    [Column("user_id")]
    public required Guid UserId { get; set; }
    
    [Required]
    [Column("auth")]
    public required string Auth { get; set; }
    
    [Required]
    [Column("endpint")]
    public required string Endpoint { get; set; }
    
    [Required]
    [Column("p256dh")]
    public required string P256DH { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public User? UserNavigation { get; set; }
}