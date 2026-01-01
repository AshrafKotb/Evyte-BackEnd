using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Eventa.ApplicationCore.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Eventa.ApplicationCore.Models.Helper;
using Eventa.ApplicationCore.Interfaces.Services;
using Eventa.ApplicationCore.Interfaces.Services.General_Information;
using Microsoft.AspNetCore.Localization;

namespace Eventa.Controllers;
//[Authorize(Roles = RoleName.Admin)]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IDesignService _designService;
    private readonly IFAQService _FAQService;
    private readonly IGeneralInformationService _generalInformationService;
    public HomeController(ILogger<HomeController> logger, IDesignService designService, IFAQService FAQService, IGeneralInformationService generalInformationService)
    {
        _logger = logger;
        _designService = designService;
        _FAQService = FAQService;
        _generalInformationService = generalInformationService;
    }


    // التعديلات على Index و IndexEn لدمجهم في action واحد
    public async Task<IActionResult> Index()
    {
        var language = GetCurrentLanguage();
        var designs = await _designService.GetAllDesignsAsync();
        var faqs = await _FAQService.GetAllActiveFAQsAsync();

        var designViewModels = designs.Take(6)
            .Select(d => new DesignViewModel
            {
                Id = d.Id,
                Name = language == "en" ? d.NameEn : d.NameAr,
                Description = language == "en" ? d.DescriptionEn : d.DescriptionAr,
                PreviewImageUrl = d.ImageUrl,
                WebsiteDemoUrl = d.WebsiteDemoUrl
            }).ToList();

        var model = new HomeIndexViewModel
        {
            Designs = designViewModels,
            FAQs = faqs.Where(x => x.HomePage).ToList(),
            CurrentLanguage = language
        };

        return View(model);
    }
    public IActionResult About()
    {
        return View();
    }
    public async Task<IActionResult> Contact()
    {
        var generalInfo = await _generalInformationService.GetGeneralInformationAsync();
        return View(generalInfo);
    }
    public async Task<IActionResult> FAQs()
    {
        var faqs = await _FAQService.GetAllActiveFAQsAsync();

        return View(faqs);
    }
    public async Task<IActionResult> Designs()
    {
        var language = GetCurrentLanguage();

        var designs = await _designService.GetAllDesignsAsync();
        var model = new HomeIndexViewModel
        {
            Designs = designs.Select(d => new DesignViewModel
            {
                Id = d.Id,
                Name = language == "en" ? d.NameEn : d.NameAr,
                Description = language == "en" ? d.DescriptionEn : d.DescriptionAr,
                PreviewImageUrl = d.ImageUrl, // يمكن تعديل هذا الحقل بناءً على تصميم قاعدة البيانات
                WebsiteDemoUrl = d.WebsiteDemoUrl
            }).ToList()
        };
        return View(model);
    }

    [HttpPost]
    public IActionResult SetLanguage(string culture, string returnUrl)
    {
        Response.Cookies.Append(
            CookieRequestCultureProvider.DefaultCookieName,
            CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
            new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddYears(1),
                IsEssential = true,
                Path = "/"
            }
        );

        return LocalRedirect(returnUrl);
    }

    private string GetCurrentLanguage()
    {
        return HttpContext.Request.Cookies[CookieRequestCultureProvider.DefaultCookieName]?.Split('|')[0]?.Split('=')[1] ?? "ar";
    }
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
