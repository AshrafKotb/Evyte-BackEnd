using Eventa.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Eventa.Domain.Entities
{
    public class Request : BaseEntity
    {
        [MaxLength(500)]
        public string? DomainUrl { get; set; }

        [Required, MaxLength(100)]
        public string WeddingSlug { get; set; } = string.Empty; // رابط الدعوة

        [MaxLength(100)]
        public string? QrCodeImageId { get; set; }

        [MaxLength(500)]
        public string? QrCodeImageUrl { get; set; }

        public Guid RequestDataId { get; set; }

        public virtual RequestData? RequestData { get; set; }

        public Guid DesignId { get; set; }

        public virtual Design? Design { get; set; }

        [Required, MaxLength(450)]
        public string UserId { get; set; } = string.Empty;

        public virtual ApplicationUser? User { get; set; }

        public virtual ICollection<RequestGalleryPhoto> GalleryPhotos { get; set; } = new List<RequestGalleryPhoto>();

        // حالات الطلبات
        public InvitationStatus Status { get; set; } = InvitationStatus.Pending;

        [MaxLength(1000)]
        public string? RejectionReason { get; set; }

        public DateTime? ApprovedDate { get; set; }
    }
}
