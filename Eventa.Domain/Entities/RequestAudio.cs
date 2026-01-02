using System.ComponentModel.DataAnnotations;

namespace Eventa.Domain.Entities
{
    /// <summary>
    /// الصوت/الموسيقى الخاصة بالدعوة
    /// علاقة 1:1 مع Request
    /// </summary>
    public class RequestAudio : BaseEntity
    {
        [Required, MaxLength(100)]
        public string AudioId { get; set; } = string.Empty;

        [Required, MaxLength(500)]
        public string AudioUrl { get; set; } = string.Empty;

        [MaxLength(200)]
        public string? AudioName { get; set; } // اسم الملف أو الأغنية

        public bool AutoPlay { get; set; } = false; // تشغيل تلقائي عند فتح الدعوة

        public Guid RequestId { get; set; }

        public virtual Request? Request { get; set; }
    }
}
