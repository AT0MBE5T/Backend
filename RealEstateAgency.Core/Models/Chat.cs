using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealEstateAgency.Core.Models;

[Table("t_chat")]
public class Chat
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [Column("type_id")]
    public required Guid TypeId { get; set; }

    [Required]
    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public ChatType? ChatTypeNavigation { get; set; }
    public ICollection<ChatMember> ChatMembersNavigation { get; set; } = [];
    public ICollection<Message> MessagesNavigation { get; set; } = [];
}