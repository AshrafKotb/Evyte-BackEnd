using Evyte.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evyte.ApplicationCore.Models.ViewModels.Categories
{
    public class CreateCategoryVM
    {

        [Required]
        [Display(Name = "Arabic Name")]
        public string NameAr { get; set; }

        [Required]
        [Display(Name = "English Name")]
        public string NameEn { get; set; }

        [Display(Name = "Arabic Description")]
        public string DescriptionAr { get; set; }

        [Display(Name = "English Description")]
        public string DescriptionEn { get; set; }

        [Display(Name = "Category Image")]
        //[FileExtensions(Extensions = "jpg,jpeg,png")]
        public IFormFile Image { get; set; }

        [Display(Name = "Sorting Number")]
        public int SortingNumber { get; set; }

    }
    public class UpdateCategoryVM
    {
        public UpdateCategoryVM() { }
        public UpdateCategoryVM(Category category)
        {
            Id = category.Id;
            NameAr = category.NameAr;
            NameEn = category.NameEn;
            DescriptionAr = category.DescriptionAr;
            DescriptionEn = category.DescriptionEn;
            SortingNumber = category.SortingNumber;
            CurrentImageUrl = category.ImageUrl;
        }
        public Guid Id { get; set; }

        [Required]
        [Display(Name = "Arabic Name")]
        public string NameAr { get; set; }

        [Required]
        [Display(Name = "English Name")]
        public string NameEn { get; set; }

        [Display(Name = "Arabic Description")]
        public string DescriptionAr { get; set; }

        [Display(Name = "English Description")]
        public string DescriptionEn { get; set; }

        [Display(Name = "Category Image")]
        //[FileExtensions(Extensions = "jpg,jpeg,png")]
        public IFormFile Image { get; set; }

        [Display(Name = "Sorting Number")]
        public int SortingNumber { get; set; }

        [HiddenInput]
        public string CurrentImageUrl { get; set; }
    }
}
