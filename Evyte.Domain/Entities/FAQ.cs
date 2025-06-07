using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evyte.Domain.Entities
{
    public class FAQ : BaseEntity
    {
        public string QuestionAr { get; set; }
        public string QuestionEn { get; set; } = string.Empty;
        public string AnswerAr { get; set; } = string.Empty;
        public string AnswerEn { get; set; } = string.Empty;
        public int SortingNumber { get; set; }
        public bool HomePage { get; set; } = false;
    }
}
