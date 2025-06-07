using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evyte.ApplicationCore.Models.ViewModels.FAQs
{
    public class FAQVM
    {
        public Guid Id { get; set; }
        public string QuestionAr { get; set; }
        public string QuestionEn { get; set; }
        public string AnswerAr { get; set; }
        public string AnswerEn { get; set; }
        public int SortingNumber { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsDeleted { get; set; }
        public bool HomePage { get; set; }
    }
}
