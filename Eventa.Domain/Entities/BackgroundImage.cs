using System.ComponentModel.DataAnnotations;

namespace Eventa.Domain.Entities
{
    public class BackgroundImage : BaseEntity
    {
        [MaxLength(100)]
        public string? ImageId { get; set; }

        [MaxLength(500)]
        public string? ImageUrl { get; set; }

        [MaxLength(200)]
        public string? NameAr { get; set; }

        [MaxLength(200)]
        public string? NameEn { get; set; }

        [MaxLength(50)]
        public string? Category { get; set; }

        public int SortingNumber { get; set; }
    }
}
