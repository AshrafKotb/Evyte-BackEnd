using System.Threading.Tasks;
using Eventa.ApplicationCore.Interfaces.Services.General_Information;
using Eventa.ApplicationCore.Models.ViewModels;
using Eventa.Domain.Entities;
using Eventa.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Eventa.Web.ViewComponents
{
    public class FooterViewComponent : ViewComponent
    {
        private readonly IGeneralInformationService _generalInformationService;

        public FooterViewComponent(IGeneralInformationService generalInformationService)
        {
            _generalInformationService = generalInformationService;
        }
        // ?? ??? ???????? ??? ??? ?????? ????????
        private string GetCurrentLanguage()
        {
            var cookie = HttpContext.Request.Cookies[CookieRequestCultureProvider.DefaultCookieName];
            return cookie?.Split('|')[0]?.Split('=')[1] ?? "ar";
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var language = HttpContext.Request.Cookies[CookieRequestCultureProvider.DefaultCookieName]?
                .Split('|')[0]?.Split('=')[1] ?? "ar";

            var generalInfo = await _generalInformationService.GetGeneralInformationAsync();
            ViewBag.CurrentLanguage = language;

            return View(generalInfo);
        }
    }
}