using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealEstateAgency.Core.Models;

[Table("t_chat_type")]
public class ChatType
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [Column("name")]
    public required string Name { get; set; }
    
    public ICollection<Chat> ChatsNavigation { get; set; } = [];
}