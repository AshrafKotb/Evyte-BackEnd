using System.ComponentModel.DataAnnotations;

namespace Eventa.Domain.Entities
{
    /// <summary>
    /// الذكريات/المناسبات - مثل الخطوبة، قراية الفاتحة، إلخ
    /// حد أقصى 5 ذكريات لكل دعوة
    /// </summary>
    public class Memory : BaseEntity
    {
        [Required, MaxLength(100)]
        public string Title { get; set; } = string.Empty;

        public DateTime EventDate { get; set; }

        [MaxLength(100)]
        public string? ImageId { get; set; }

        [MaxLength(500)]
        public string? ImageUrl { get; set; }

        public int DisplayOrder { get; set; }

        public Guid RequestId { get; set; }

        public virtual Request? Request { get; set; }
    }
}
