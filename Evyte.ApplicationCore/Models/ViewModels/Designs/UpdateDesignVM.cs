using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using Evyte.Domain.Entities;

namespace Evyte.ApplicationCore.Models.ViewModels.Designs
{
    public class UpdateDesignVM
    {
        public UpdateDesignVM() { }

        public UpdateDesignVM(Design design)
        {
            Id = design.Id;
            NameAr = design.NameAr;
            NameEn = design.NameEn;
            DescriptionAr = design.DescriptionAr;
            DescriptionEn = design.DescriptionEn;
            SortingNumber = design.SortingNumber;
            CategoryId = design.CategoryId;
            //WebsiteDemoUrl = design.WebsiteDemoUrl;
            CurrentImageUrl = design.ImageUrl;
            TemplateName = design.TemplateName;
        }

        public Guid Id { get; set; }

        [Required(ErrorMessage = "الاسم العربي مطلوب")]
        public string NameAr { get; set; }

        [Required(ErrorMessage = "الاسم الإنجليزي مطلوب")]
        public string NameEn { get; set; }

        public string DescriptionAr { get; set; }
        public string DescriptionEn { get; set; }

        [Required(ErrorMessage = "رقم الترتيب مطلوب")]
        [Range(1, int.MaxValue, ErrorMessage = "رقم الترتيب يجب أن يكون أكبر من 0")]
        public int SortingNumber { get; set; }

        [Required(ErrorMessage = "معرف القسم مطلوب")]
        public Guid CategoryId { get; set; }

        //public string WebsiteDemoUrl { get; set; }

        [Display(Name = "Design Image")]
        public IFormFile Image { get; set; }
        public string CurrentImageUrl { get; set; }
        [Required(ErrorMessage = "اسم القالب مطلوب")]
        [RegularExpression(@"^[a-zA-Z0-9\-]+$", ErrorMessage = "اسم القالب يجب أن يحتوي على حروف، أرقام، أو شرطات فقط بدون مسافات")]
        public string TemplateName { get; set; } // حقل لاسم القالب
    }
}