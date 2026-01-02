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

        // حالات الطلبات
        public InvitationStatus Status { get; set; } = InvitationStatus.Pending;

        [MaxLength(1000)]
        public string? RejectionReason { get; set; }

        public DateTime? ApprovedDate { get; set; }

        #region Featured (أعمالنا السابقة)
        /// <summary>
        /// هل يظهر في قسم "أعمالنا السابقة" على الصفحة الرئيسية
        /// </summary>
        public bool IsFeatured { get; set; } = false;

        /// <summary>
        /// ترتيب العرض في قسم الأعمال السابقة
        /// </summary>
        public int? FeaturedOrder { get; set; }
        #endregion

        #region Optional Sections Flags
        /// <summary>
        /// هل الدعوة تحتوي على ذكريات/مناسبات
        /// </summary>
        public bool HasMemories { get; set; } = false;

        /// <summary>
        /// هل الدعوة تحتوي على إهداءات
        /// </summary>
        public bool HasDedications { get; set; } = false;

        /// <summary>
        /// هل الدعوة تحتوي على ألبوم صور
        /// </summary>
        public bool HasGallery { get; set; } = false;

        /// <summary>
        /// هل الدعوة تحتوي على صوت/موسيقى
        /// </summary>
        public bool HasAudio { get; set; } = false;
        #endregion

        #region Navigation Properties
        /// <summary>
        /// صور الألبوم (حد أقصى 10 صور)
        /// </summary>
        public virtual ICollection<RequestGalleryPhoto> GalleryPhotos { get; set; } = new List<RequestGalleryPhoto>();

        /// <summary>
        /// الذكريات/المناسبات (حد أقصى 5)
        /// </summary>
        public virtual ICollection<Memory> Memories { get; set; } = new List<Memory>();

        /// <summary>
        /// الإهداءات (بدون حد أقصى)
        /// </summary>
        public virtual ICollection<Dedication> Dedications { get; set; } = new List<Dedication>();

        /// <summary>
        /// الصوت/الموسيقى (اختياري)
        /// </summary>
        public virtual RequestAudio? Audio { get; set; }
        #endregion
    }
}
