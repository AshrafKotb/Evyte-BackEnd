using System.Threading.Tasks;
using Evyte.ApplicationCore.Interfaces.Services.General_Information;
using Evyte.ApplicationCore.Models.ViewModels;
using Evyte.Domain.Entities;
using Evyte.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Evyte.Web.ViewComponents
{
    public class NavBarViewComponent : ViewComponent
    {
        private readonly IGeneralInformationService _generalInformationService;

        public NavBarViewComponent(IGeneralInformationService generalInformationService)
        {
            _generalInformationService = generalInformationService;
        }
        // في كلا الملفين، أضف هذه الدالة المساعدة
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