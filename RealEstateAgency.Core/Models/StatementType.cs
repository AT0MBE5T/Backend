using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealEstateAgency.Core.Models
{
    [Table("t_statement_type")]
    public class StatementType
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [Column("name")]
        [StringLength(maximumLength: 30, MinimumLength = 3)]
        public required string Name { get; set; }

        public ICollection<Statement> StatementsNavigation { get; set; } = [];
    }
}
