using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealEstateAgency.Core.Entities;

[Table("t_chat_member")]
public class ChatMember
{
    [Required]
    [Column("chat_id")]
    public Guid ChatId { get; set; }

    [Required]
    [Column("user_id")]
    public required Guid UserId { get; set; }

    [Required]
    [Column("joined_at")]
    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
    
    public Chat? ChatNavigation { get; set; }
    public User? UserNavigation { get; set; }
}