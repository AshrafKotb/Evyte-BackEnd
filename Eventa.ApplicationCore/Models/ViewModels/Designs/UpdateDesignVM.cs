using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using Eventa.Domain.Entities;

namespace Eventa.ApplicationCore.Models.ViewModels.Designs
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

        [Required(ErrorMessage = "????? ?????? ?????")]
        public string NameAr { get; set; }

        [Required(ErrorMessage = "????? ????????? ?????")]
        public string NameEn { get; set; }

        public string DescriptionAr { get; set; }
        public string DescriptionEn { get; set; }

        [Required(ErrorMessage = "??? ??????? ?????")]
        [Range(1, int.MaxValue, ErrorMessage = "??? ??????? ??? ?? ???? ???? ?? 0")]
        public int SortingNumber { get; set; }

        [Required(ErrorMessage = "???? ????? ?????")]
        public Guid CategoryId { get; set; }

        //public string WebsiteDemoUrl { get; set; }

        [Display(Name = "Design Image")]
        public IFormFile Image { get; set; }
        public string? CurrentImageUrl { get; set; }
        [Required(ErrorMessage = "??? ?????? ?????")]
        [RegularExpression(@"^[a-zA-Z0-9\-]+$", ErrorMessage = "??? ?????? ??? ?? ????? ??? ????? ?????? ?? ????? ??? ???? ??????")]
        public string TemplateName { get; set; } // ??? ???? ??????
    }
}