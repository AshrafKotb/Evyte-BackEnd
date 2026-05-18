using System.ComponentModel.DataAnnotations;

namespace Eventa.ApplicationCore.Models.ViewModels
{
    public class EditorSubmitDTO
    {
        // Contact Info
        [Required(ErrorMessage = "الاسم الكامل مطلوب")]
        [MinLength(2, ErrorMessage = "الاسم الكامل يجب أن يكون على الأقل حرفين")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "رقم الهاتف مطلوب")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "رقم الواتساب مطلوب")]
        public string WhatsAppNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
        [EmailAddress(ErrorMessage = "يرجى إدخال بريد إلكتروني صالح")]
        public string Email { get; set; } = string.Empty;

        // Design
        [Required]
        public Guid DesignId { get; set; }

        // Couple
        [Required(ErrorMessage = "اسم العريس مطلوب")]
        [MinLength(2, ErrorMessage = "اسم العريس يجب أن يكون على الأقل حرفين")]
        public string GroomName { get; set; } = string.Empty;

        [Required(ErrorMessage = "اسم العروسة مطلوب")]
        [MinLength(2, ErrorMessage = "اسم العروسة يجب أن يكون على الأقل حرفين")]
        public string BrideName { get; set; } = string.Empty;
        public string? GroomImageUrl { get; set; }
        public string? GroomImageId { get; set; }
        public string? BrideImageUrl { get; set; }
        public string? BrideImageId { get; set; }
        public string? GroomFacebook { get; set; }
        public string? GroomInstagram { get; set; }
        public string? GroomX { get; set; }
        public string? BrideFacebook { get; set; }
        public string? BrideInstagram { get; set; }
        public string? BrideX { get; set; }

        // Main Image
        public string? MainSliderImageUrl { get; set; }
        public string? MainSliderImageId { get; set; }

        // Event
        public string EventDate { get; set; } = string.Empty;
        public string EventTimeFrom { get; set; } = "18:00";
        public string EventTimeTo { get; set; } = "23:00";
        public string EventPlaceName { get; set; } = string.Empty;
        public string EventAddress { get; set; } = string.Empty;
        public string? LocationUrl { get; set; }
        public string? EventPlaceImageUrl { get; set; }
        public string? EventPlaceImageId { get; set; }

        // Gallery (URLs already uploaded)
        public List<EditorGalleryPhotoDTO>? GalleryPhotos { get; set; }

        // Memories
        public List<EditorMemoryDTO>? Memories { get; set; }

        // Dedications
        public List<EditorDedicationDTO>? Dedications { get; set; }

        // Audio
        public string? AudioUrl { get; set; }
        public string? AudioId { get; set; }
        public string? AudioName { get; set; }
        public bool AudioAutoPlay { get; set; }

        // Custom Content
        public string? CustomSentence1 { get; set; }
        public string? CustomSentence2 { get; set; }
        public string? BackgroundImageUrl { get; set; }
        public string? BackgroundImageId { get; set; }
        public Guid? BackgroundImageLibraryId { get; set; }

        public Guid? SplashTemplateId { get; set; }
    }

    public class EditorMemoryDTO
    {
        public string Title { get; set; } = string.Empty;
        public DateTime EventDate { get; set; }
        public string? ImageUrl { get; set; }
        public string? ImageId { get; set; }
        public int DisplayOrder { get; set; }
    }

    public class EditorDedicationDTO
    {
        public string PersonName { get; set; } = string.Empty;
        public string? Relationship { get; set; }
        public string Message { get; set; } = string.Empty;
        public int DisplayOrder { get; set; }
    }

    public class EditorGalleryPhotoDTO
    {
        public string PhotoUrl { get; set; } = string.Empty;
        public string PhotoId { get; set; } = string.Empty;
    }
}
