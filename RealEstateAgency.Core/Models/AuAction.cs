using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealEstateAgency.Core.Models
{
    [Table("t_au_action")]
    public class AuAction
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [Column("name")]
        [StringLength(maximumLength: 30, MinimumLength = 5)]
        public required string Name { get; set; }

        public ICollection<AuHistory> AuHistoriesNavigation { get; set; } = [];
    }
}
