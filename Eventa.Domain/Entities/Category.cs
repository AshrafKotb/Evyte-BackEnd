using System.ComponentModel.DataAnnotations;

namespace Eventa.Domain.Entities
{
    public class Category : BaseEntity
    {
        [Required, MaxLength(100)]
        public string NameAr { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string NameEn { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? DescriptionAr { get; set; }

        [MaxLength(500)]
        public string? DescriptionEn { get; set; }

        [MaxLength(100)]
        public string? ImageId { get; set; }

        [MaxLength(500)]
        public string? ImageUrl { get; set; }

        public int SortingNumber { get; set; }

        public virtual ICollection<Design> Designs { get; set; } = new List<Design>();
    }
}
