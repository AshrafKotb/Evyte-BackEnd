using System.ComponentModel.DataAnnotations;

namespace Eventa.Domain.Entities
{
    public class SplashTemplate : BaseEntity
    {
        [Required, MaxLength(200)]
        public string NameAr { get; set; } = string.Empty;

        [Required, MaxLength(200)]
        public string NameEn { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? DescriptionAr { get; set; }

        [MaxLength(500)]
        public string? DescriptionEn { get; set; }

        // اسم ملف الـ Partial تحت Views/Shared/splashes/_{PartialName}.cshtml
        [Required, MaxLength(100)]
        public string PartialName { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? ThumbnailUrl { get; set; }

        [MaxLength(100)]
        public string? ThumbnailId { get; set; }

        // مدة العرض بالميلي ثانية قبل الإخفاء التلقائي (لو 0 يفضل ظاهر لحد ما المستخدم يفتحه)
        public int DurationMs { get; set; } = 3000;

        // هل يحتاج تفاعل من المستخدم (tap to open) أو يختفي تلقائياً
        public bool RequiresInteraction { get; set; } = false;

        public bool IsActive { get; set; } = true;

        public bool IsDefault { get; set; } = false;

        public int SortingNumber { get; set; }
    }
}
