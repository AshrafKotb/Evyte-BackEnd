using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Evyte.ApplicationCore.Models.ViewModels
{
    public class CreateInvitationVM
    {
        public Guid DesignId { get; set; }

        // Contact Information
        [Required(ErrorMessage = "الاسم الكامل مطلوب")]
        [MinLength(2, ErrorMessage = "الاسم الكامل يجب أن يكون على الأقل حرفين")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "رقم الهاتف مطلوب")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
        [EmailAddress(ErrorMessage = "يرجى إدخال بريد إلكتروني صالح")]
        public string Email { get; set; }

        // Groom Information
        [Required(ErrorMessage = "اسم العريس مطلوب")]
        [MinLength(2, ErrorMessage = "اسم العريس يجب أن يكون على الأقل حرفين")]
        public string GroomName { get; set; }

        //[Required(ErrorMessage = "صورة العريس مطلوبة")]
        public IFormFile? GroomImage { get; set; }

        // Social media fields (optional)
        public string? GroomFacebook { get; set; }
        public string? GroomInstagram { get; set; }
        public string? GroomX { get; set; }

        // Bride Information
        [Required(ErrorMessage = "اسم العروس مطلوب")]
        [MinLength(2, ErrorMessage = "اسم العروس يجب أن يكون على الأقل حرفين")]
        public string BrideName { get; set; }

        //[Required(ErrorMessage = "صورة العروس مطلوبة")]
        public IFormFile? BrideImage { get; set; }

        // Social media fields (optional)
        public string? BrideFacebook { get; set; }
        public string? BrideInstagram { get; set; }
        public string? BrideX { get; set; }

        // Event Details
        [Required(ErrorMessage = "تاريخ الحدث مطلوب")]
        public DateTime EventDate { get; set; }

        [Required(ErrorMessage = "وقت البدء مطلوب")]
        public TimeSpan EventTimeFrom { get; set; }

        [Required(ErrorMessage = "وقت الانتهاء مطلوب")]
        public TimeSpan EventTimeTo { get; set; }

        [Required(ErrorMessage = "اسم المكان مطلوب")]
        public string EventPlaceName { get; set; }

        //[Required(ErrorMessage = "صورة المكان مطلوبة")]
        public IFormFile? EventPlaceImage { get; set; }

        [Required(ErrorMessage = "عنوان المكان مطلوب")]
        public string EventAddress { get; set; }

        [Required(ErrorMessage = "رابط الموقع مطلوب")]
        [Url(ErrorMessage = "يرجى إدخال رابط صالح")]
        public string LocationUrl { get; set; }

        // Gallery
        [Required(ErrorMessage = "صورة السلايدر الرئيسية مطلوبة")]
        public IFormFile MainSliderImage { get; set; }

        [Required(ErrorMessage = "يجب تحميل صورة واحدة على الأقل للمعرض")]
        public IFormFileCollection GalleryPhotos { get; set; }
        // New properties for avatar selections
        public string GroomAvatar { get; set; }
        public string BrideAvatar { get; set; }
    }
}