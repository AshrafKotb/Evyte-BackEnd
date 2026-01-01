using System.ComponentModel.DataAnnotations;

namespace Eventa.Domain.Entities
{
    public class FAQ : BaseEntity
    {
        [Required, MaxLength(500)]
        public string QuestionAr { get; set; } = string.Empty;

        [Required, MaxLength(500)]
        public string QuestionEn { get; set; } = string.Empty;

        [Required, MaxLength(2000)]
        public string AnswerAr { get; set; } = string.Empty;

        [Required, MaxLength(2000)]
        public string AnswerEn { get; set; } = string.Empty;

        public int SortingNumber { get; set; }

        public bool HomePage { get; set; } = false;
    }
}
