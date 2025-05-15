using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace Evyte.ApplicationCore.Models.ViewModels.Designs
{
    public class CreateDesignVM
    {
        [Required(ErrorMessage = "الاسم العربي مطلوب")]
        public string NameAr { get; set; }

        [Required(ErrorMessage = "الاسم الإنجليزي مطلوب")]
        public string NameEn { get; set; }

        public string DescriptionAr { get; set; }
        public string DescriptionEn { get; set; }
        [Display(Name = "Design Image")]
        //[FileExtensions(Extensions = "jpg,jpeg,png")]
        public IFormFile Image { get; set; }
        [Required(ErrorMessage = "رقم الترتيب مطلوب")]
        [Range(1, int.MaxValue, ErrorMessage = "رقم الترتيب يجب أن يكون أكبر من 0")]
        public int SortingNumber { get; set; }

        [Required(ErrorMessage = "معرف القسم مطلوب")]
        public Guid CategoryId { get; set; }

        //public string WebsiteDemoUrl { get; set; }

        [Required(ErrorMessage = "اسم القالب مطلوب")]
        [RegularExpression(@"^[a-zA-Z0-9\-]+$", ErrorMessage = "اسم القالب يجب أن يحتوي على حروف، أرقام، أو شرطات فقط بدون مسافات")]
        public string TemplateName { get; set; } // حقل لاسم القالب
    }
}