using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealEstateAgency.Core.Models
{
    [Table("t_property")]
    public class Property
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [Column("property_type_id")]
        public required Guid PropertyTypeId { get; set; }

        [Required]
        [Column("location")]
        [StringLength(maximumLength: 100, MinimumLength = 5)]
        public required string Location { get; set; }

        [Required]
        [Column("area")]
        [Range(1, double.MaxValue)]
        public required double Area { get; set; }

        [Range(0, 200)]
        [Column("floors")]
        public int Floors { get; set; }

        [Range(0, 1000)]
        [Column("rooms")]
        public int Rooms { get; set; }

        [Required]
        [Column("description")]
        [StringLength(maximumLength: 500, MinimumLength = 5)]
        public required string Description { get; set; }

        public ICollection<Image> ImagesNavigation { get; set; } = [];
        public PropertyType? PropertyTypeNavigation { get; set; }
        public Statement? StatementNavigation { get; set; }
    }
}
