using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealEstateAgency.Core.Models;

[Table("t_image")]
public class Image
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Column("property_id")]
    public Guid? PropertyId { get; set; }
    
    [Required]
    [Column("photo_url")]
    public required string PhotoUrl { get; set; }
    
    [Required]
    [Column("public_id")]
    public required string PublicId { get; set; }
    
    [Required]
    [Column("order_index")]
    public required int OrderIndex { get; set; }
    
    public Property? Property { get; set; }
}