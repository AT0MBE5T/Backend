using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealEstateAgency.Core.Models;

[Table("t_message")]
public class Message
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [Column("chat_id")]
    public required Guid ChatId { get; set; }
    
    [Required]
    [Column("sender_id")]
    public required Guid SenderId { get; set; }
    
    [Required]
    [Column("content")]
    public required string Content { get; set; }

    [Required]
    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Required]
    [Column("is_read")]
    public bool IsRead { get; set; } = false;
    
    public User? UserNavigation { get; set; }
    public Chat? ChatNavigation { get; set; }
}