using System.ComponentModel.DataAnnotations;

namespace Eventa.Domain.Entities
{
    public class RequestGalleryPhoto : BaseEntity
    {
        [Required, MaxLength(100)]
        public string PhotoId { get; set; } = string.Empty;

        [Required, MaxLength(500)]
        public string PhotoUrl { get; set; } = string.Empty;

        public Guid RequestId { get; set; }

        public virtual Request? Request { get; set; }
    }
}
