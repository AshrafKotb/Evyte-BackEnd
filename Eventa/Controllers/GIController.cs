using Microsoft.AspNetCore.Mvc;
using Eventa.ApplicationCore.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Eventa.ApplicationCore.Models.Helper;
using Eventa.ApplicationCore.Interfaces.Services.General_Information;
using Eventa.Domain.Entities;
using System.Threading.Tasks;
using Eventa.ApplicationCore.Models.ViewModels.General_Information;

namespace Eventa.Controllers
{
    [Authorize(Roles = RoleName.Admin)]
    public class GIController : Controller
    {
        private readonly IGeneralInformationService _generalInformationService;

        public GIController(IGeneralInformationService generalInformationService)
        {
            _generalInformationService = generalInformationService;
        }

        //[AllowAnonymous]
        public async Task<IActionResult> TermsAndConditions()
        {
            var generalInfo = await GetGeneralInformationAsync();
            var viewModel = new TermsAndConditionsViewModel
            {
                TermsAndConditionsAr = generalInfo.TermsAndConditionsAr,
                TermsAndConditionsEn = generalInfo.TermsAndConditionsEn
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TermsAndConditions(TermsAndConditionsViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            try
            {
                var generalInfo = new GeneralInformation
                {
                    TermsAndConditionsAr = viewModel.TermsAndConditionsAr,
                    TermsAndConditionsEn = viewModel.TermsAndConditionsEn
                };

                await _generalInformationService.UpdateTermsAndConditionsAsync(generalInfo);
                TempData["SuccessMessage"] = "تم تحديث الشروط والأحكام بنجاح.";
                return RedirectToAction("TermsAndConditions");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "حدث خطأ أثناء تحديث الشروط والأحكام. حاول مرة أخرى.");
                return View(viewModel);
            }
        }

        //[AllowAnonymous]
        public async Task<IActionResult> ContactInfo()
        {
            var generalInfo = await GetGeneralInformationAsync();
            var viewModel = new ContactInformationViewModel
            {
                AddressAr = generalInfo.AddressAr,
                AddressEn = generalInfo.AddressEn,
                Email = generalInfo.Email,
                Email2 = generalInfo.Email2,
                PhoneNumber = generalInfo.PhoneNumber,
                PhoneNumber2 = generalInfo.PhoneNumber2,
                FaceBook = generalInfo.FaceBook,
                Instagram = generalInfo.Instagram,
                X = generalInfo.X,
                Tiktok = generalInfo.Tiktok,
                Youtube = generalInfo.Youtube,
                WhatsApp = generalInfo.WhatsApp,
                Snapchat = generalInfo.Snapchat
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ContactInfo(ContactInformationViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            try
            {
                var generalInfo = new GeneralInformation
                {
                    AddressAr = viewModel.AddressAr,
                    AddressEn = viewModel.AddressEn,
                    Email = viewModel.Email,
                    Email2 = viewModel.Email2,
                    PhoneNumber = viewModel.PhoneNumber,
                    PhoneNumber2 = viewModel.PhoneNumber2,
                    FaceBook = viewModel.FaceBook,
                    Instagram = viewModel.Instagram,
                    X = viewModel.X,
                    Tiktok = viewModel.Tiktok,
                    Youtube = viewModel.Youtube,
                    WhatsApp = viewModel.WhatsApp,
                    Snapchat = viewModel.Snapchat
                };

                await _generalInformationService.UpdateContactInformationAsync(generalInfo);
                TempData["SuccessMessage"] = "تم تحديث بيانات التواصل بنجاح.";
                return RedirectToAction("ContactInfo");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "حدث خطأ أثناء تحديث بيانات التواصل. حاول مرة أخرى.");
                return View(viewModel);
            }
        }

        private async Task<GeneralInformation> GetGeneralInformationAsync()
        {
            var generalInfo = await _generalInformationService.GetGeneralInformationAsync();
            if (generalInfo == null)
            {
                try
                {
                    generalInfo = await _generalInformationService.CreateDefaultGeneralInformationAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error creating default GeneralInformation: {ex.Message}");
                    throw new Exception("حدث خطأ أثناء جلب البيانات . حاول مرة أخرى.");
                }
            }
            return generalInfo;
        }
    }
}