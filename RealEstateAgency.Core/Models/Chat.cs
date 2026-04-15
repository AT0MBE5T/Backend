using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

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
    
    [Column("announcement_id")]
    public Guid? AnnouncementId { get; set; }
    
    public ChatType? ChatTypeNavigation { get; set; }
    public Announcement? AnnouncementNavigation { get; set; }
    public ICollection<ChatMember> ChatMembersNavigation { get; set; } = [];
    public ICollection<Message> MessagesNavigation { get; set; } = [];
}