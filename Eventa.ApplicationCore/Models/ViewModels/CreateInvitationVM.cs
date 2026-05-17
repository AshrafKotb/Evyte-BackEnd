using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Eventa.ApplicationCore.Models.ViewModels
{
    public class CreateInvitationVM
    {
        public Guid DesignId { get; set; }

        #region Contact Information
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
        #endregion

        #region Groom Information
        [Required(ErrorMessage = "اسم العريس مطلوب")]
        [MinLength(2, ErrorMessage = "اسم العريس يجب أن يكون على الأقل حرفين")]
        public string GroomName { get; set; } = string.Empty;

        public IFormFile? GroomImage { get; set; }
        public string? GroomAvatar { get; set; }

        // Social media fields (optional)
        public string? GroomFacebook { get; set; }
        public string? GroomInstagram { get; set; }
        public string? GroomX { get; set; }
        #endregion

        #region Bride Information
        [Required(ErrorMessage = "اسم العروس مطلوب")]
        [MinLength(2, ErrorMessage = "اسم العروس يجب أن يكون على الأقل حرفين")]
        public string BrideName { get; set; } = string.Empty;

        public IFormFile? BrideImage { get; set; }
        public string? BrideAvatar { get; set; }

        // Social media fields (optional)
        public string? BrideFacebook { get; set; }
        public string? BrideInstagram { get; set; }
        public string? BrideX { get; set; }
        #endregion

        #region Event Details
        [Required(ErrorMessage = "تاريخ الحدث مطلوب")]
        public DateTime EventDate { get; set; }

        [Required(ErrorMessage = "وقت البدء مطلوب")]
        public TimeSpan EventTimeFrom { get; set; }

        [Required(ErrorMessage = "وقت الانتهاء مطلوب")]
        public TimeSpan EventTimeTo { get; set; }

        [Required(ErrorMessage = "اسم المكان مطلوب")]
        public string EventPlaceName { get; set; } = string.Empty;

        public IFormFile? EventPlaceImage { get; set; }

        [Required(ErrorMessage = "عنوان المكان مطلوب")]
        public string EventAddress { get; set; } = string.Empty;

        [Required(ErrorMessage = "رابط الموقع مطلوب")]
        [Url(ErrorMessage = "يرجى إدخال رابط صالح")]
        public string LocationUrl { get; set; } = string.Empty;
        #endregion

        #region Photos
        [Required(ErrorMessage = "صورة السلايدر الرئيسية مطلوبة")]
        public IFormFile MainSliderImage { get; set; } = null!;

        public IFormFileCollection? GalleryPhotos { get; set; }
        #endregion

        #region Custom Content
        [MaxLength(500)]
        public string? CustomSentence1 { get; set; }

        [MaxLength(500)]
        public string? CustomSentence2 { get; set; }

        public Guid? BackgroundImageLibraryId { get; set; }
        public IFormFile? BackgroundImage { get; set; }

        // اختيار العميل لسبلاش من المكتبة (اختياري)
        public Guid? SplashTemplateId { get; set; }
        #endregion

        #region Optional Sections Flags
        public bool HasMemories { get; set; } = false;
        public bool HasDedications { get; set; } = false;
        public bool HasGallery { get; set; } = false;
        public bool HasAudio { get; set; } = false;
        #endregion

        #region Memories (Max 5)
        public List<MemoryItemVM>? Memories { get; set; }
        #endregion

        #region Dedications (Unlimited)
        public List<DedicationItemVM>? Dedications { get; set; }
        #endregion

        #region Audio
        public IFormFile? AudioFile { get; set; }
        public string? AudioName { get; set; }
        public bool AudioAutoPlay { get; set; } = false;
        #endregion
    }

    /// <summary>
    /// ViewModel للذكريات/المناسبات
    /// </summary>
    public class MemoryItemVM
    {
        [Required(ErrorMessage = "عنوان الذكرى مطلوب")]
        [MaxLength(100)]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "تاريخ الذكرى مطلوب")]
        public DateTime EventDate { get; set; }

        public IFormFile? Image { get; set; }

        public int DisplayOrder { get; set; }
    }

    /// <summary>
    /// ViewModel للإهداءات
    /// </summary>
    public class DedicationItemVM
    {
        [Required(ErrorMessage = "اسم الشخص مطلوب")]
        [MaxLength(100)]
        public string PersonName { get; set; } = string.Empty;

        [MaxLength(50)]
        public string? Relationship { get; set; }

        [Required(ErrorMessage = "نص الإهداء مطلوب")]
        [MaxLength(1000)]
        public string Message { get; set; } = string.Empty;

        public int DisplayOrder { get; set; }
    }
}