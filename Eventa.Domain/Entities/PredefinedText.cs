using System.ComponentModel.DataAnnotations;

namespace Eventa.Domain.Entities
{
    public class PredefinedText : BaseEntity
    {
        [Required, MaxLength(500)]
        public string TextAr { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? TextEn { get; set; }

        [Required, MaxLength(50)]
        public string TextType { get; set; } = string.Empty;

        public int SortingNumber { get; set; }
    }
}
