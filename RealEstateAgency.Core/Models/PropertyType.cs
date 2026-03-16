using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealEstateAgency.Core.Models
{
    [Table("t_property_type")]
    public class PropertyType
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [Column("name")]
        [StringLength(maximumLength: 30, MinimumLength = 5)]
        public required string Name { get; set; }

        public ICollection<Property> PropertiesNavigation { get; set; } = [];
    }
}
