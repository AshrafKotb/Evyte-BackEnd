using Evyte.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evyte.ApplicationCore.Models.ViewModels.FAQs
{
    public class CreateFAQVM
    {

        [Required]
        [Display(Name = "Arabic Question")]
        public string QuestionAr { get; set; }

        [Required]
        [Display(Name = "English Question")]
        public string QuestionEn { get; set; }

        [Display(Name = "Arabic Answer")]
        public string AnswerAr { get; set; }

        [Display(Name = "English Answer")]
        public string AnswerEn { get; set; }

        [Display(Name = "Sorting Number")]
        public int SortingNumber { get; set; }
        public bool HomePage { get; set; }

    }
    public class UpdateFAQVM
    {
        public UpdateFAQVM() { }
        public UpdateFAQVM(FAQ FAQ)
        {
            Id = FAQ.Id;
            QuestionAr = FAQ.QuestionAr;
            QuestionEn = FAQ.QuestionEn;
            AnswerAr = FAQ.AnswerAr;
            AnswerEn = FAQ.AnswerEn;
            SortingNumber = FAQ.SortingNumber;
            HomePage = FAQ.HomePage;
        }
        public Guid Id { get; set; }

        [Required]
        [Display(Name = "Arabic Question")]
        public string QuestionAr { get; set; }

        [Required]
        [Display(Name = "English Question")]
        public string QuestionEn { get; set; }

        [Display(Name = "Arabic Answer")]
        public string AnswerAr { get; set; }

        [Display(Name = "English Answer")]
        public string AnswerEn { get; set; }

        [Display(Name = "Sorting Number")]
        public int SortingNumber { get; set; }
        public bool HomePage { get; set; }
    }
}
