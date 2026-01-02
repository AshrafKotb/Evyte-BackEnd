using System.ComponentModel.DataAnnotations;

namespace Eventa.Domain.Entities
{
    /// <summary>
    /// الإهداءات - كلمات من الأهل والأصدقاء
    /// بدون حد أقصى
    /// </summary>
    public class Dedication : BaseEntity
    {
        [Required, MaxLength(100)]
        public string PersonName { get; set; } = string.Empty;

        [MaxLength(50)]
        public string? Relationship { get; set; } // مثال: أخو العريس، صديقة العروسة

        [Required, MaxLength(1000)]
        public string Message { get; set; } = string.Empty;

        public int DisplayOrder { get; set; }

        public Guid RequestId { get; set; }

        public virtual Request? Request { get; set; }
    }
}
