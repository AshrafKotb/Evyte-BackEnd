using System.ComponentModel.DataAnnotations;

namespace Eventa.Domain.Entities
{
    public class Design : BaseEntity
    {
        [Required, MaxLength(100)]
        public string NameAr { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string NameEn { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string TemplateName { get; set; } = string.Empty; // اسم ملف الـ Template

        [MaxLength(500)]
        public string DescriptionAr { get; set; } = string.Empty;

        [MaxLength(500)]
        public string DescriptionEn { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? ImageId { get; set; }

        [MaxLength(500)]
        public string? ImageUrl { get; set; }

        [MaxLength(500)]
        public string? WebsiteDemoUrl { get; set; }

        public int SortingNumber { get; set; }

        public Guid CategoryId { get; set; }

        public virtual Category? Category { get; set; }

        public virtual ICollection<Request> Requests { get; set; } = new List<Request>();
    }
}
