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

        [Required(ErrorMessage = "رقم الترتيب مطلوب")]
        [Range(1, int.MaxValue, ErrorMessage = "رقم الترتيب يجب أن يكون أكبر من 0")]
        public int SortingNumber { get; set; }

        [Required(ErrorMessage = "معرف القسم مطلوب")]
        public Guid CategoryId { get; set; }

        public string WebsiteDemoUrl { get; set; }

        //public IFormFile Image { get; set; }
    }
}