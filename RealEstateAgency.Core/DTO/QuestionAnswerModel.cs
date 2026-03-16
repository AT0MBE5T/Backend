using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateAgency.Core.DTO
{
    public class QuestionAnswerModel
    {
        public Guid QuestionId { get; set; }
        public Guid? AnswerId { get; set; }
        public required DateTime CreatedAtQuestion { get; set; }
        public required string CreatedByQuestion { get; set; }
        public DateTime? CreatedAtAnswer { get; set; }
        public string CreatedByAnswer { get; set; } = string.Empty;
        public string TextAnswer { get; set; } = string.Empty;
        public required string TextQuestion { get; set; }
        public required Guid AnnouncementId { get; set; }
    }
}
