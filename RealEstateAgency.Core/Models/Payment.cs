using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealEstateAgency.Core.Models;

[Table("t_payment")]
public class Payment
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; } = Guid.NewGuid();
    
    [Required]
    [Column("customer_id")]
    public Guid CustomerId { get; set; }
    
    [Required]
    [Column("announcement_id")]
    public Guid AnnouncementId { get; set; }
    
    public User? CustomerNavigation { get; set; }
    public Announcement? AnnouncementNavigation { get; set; }
}