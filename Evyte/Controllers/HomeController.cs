using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Evyte.ApplicationCore.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Evyte.ApplicationCore.Models.Helper;
using Evyte.ApplicationCore.Interfaces.Services;

namespace Evyte.Controllers;
//[Authorize(Roles = RoleName.Admin)]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IDesignService _designService;
    private readonly IFAQService _FAQService;

    public HomeController(ILogger<HomeController> logger, IDesignService designService, IFAQService FAQService)
    {
        _logger = logger;
        _designService = designService;
        _FAQService = FAQService;
    }


    //public async Task<IActionResult> Index()
    //{
    //    var designs = await _designService.GetAllDesignsAsync();
    //    var model = new HomeIndexViewModel
    //    {
    //        Designs = designs.Select(d => new DesignViewModel
    //        {
    //            Id = d.Id,
    //            NameAr = d.NameAr,
    //            DescriptionAr = d.DescriptionAr,
    //            PreviewImageUrl = d.WebsiteDemoUrl, // يمكن تعديل هذا الحقل بناءً على تصميم قاعدة البيانات
    //            WebsiteDemoUrl = d.WebsiteDemoUrl
    //        }).ToList()
    //    };
    //    return View(model);
    //}
    public async Task<IActionResult> Index()
    {
        var designs = await _designService.GetAllDesignsAsync();
        var faqs = await _FAQService.GetAllActiveFAQsAsync();

        // إنشاء قائمة تحتوي على كل تصميم مكرر 10 مرات
        var repeatedDesigns = designs.Take(6)
            .Select((d, index) => new DesignViewModel
            {
                Id = d.Id,
                Name = d.NameEn,
                Description = d.DescriptionEn,
                PreviewImageUrl = d.ImageUrl,
                WebsiteDemoUrl = d.WebsiteDemoUrl
            }).ToList();

        var model = new HomeIndexViewModel
        {
            Designs = repeatedDesigns,
            FAQs = faqs.Where(x => x.HomePage).ToList()
        };

        return View(model);
    }

    public async Task<IActionResult> IndexEn()
    {
        var designs = await _designService.GetAllDesignsAsync();
        var model = new HomeIndexViewModel
        {
            Designs = designs.Select(d => new DesignViewModel
            {
                Id = d.Id,
                Name = d.NameAr,
                Description = d.DescriptionAr,
                PreviewImageUrl = d.ImageUrl, // يمكن تعديل هذا الحقل بناءً على تصميم قاعدة البيانات
                WebsiteDemoUrl = d.WebsiteDemoUrl
            }).ToList()
        };
        return View(model);
    }

    public IActionResult About()
    {
        return View();
    }
    public IActionResult Contact()
    {
        return View();
    }
    public async Task<IActionResult> Questions()
    {
        var faqs = await _FAQService.GetAllActiveFAQsAsync();

        return View(faqs);
    }
    public async Task<IActionResult> Templates()
    {
        var designs = await _designService.GetAllDesignsAsync();
        var model = new HomeIndexViewModel
        {
            Designs = designs.Select(d => new DesignViewModel
            {
                Id = d.Id,
                Name = d.NameAr,
                Description = d.DescriptionAr,
                PreviewImageUrl = d.ImageUrl, // يمكن تعديل هذا الحقل بناءً على تصميم قاعدة البيانات
                WebsiteDemoUrl = d.WebsiteDemoUrl
            }).ToList()
        };
        return View(model);
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
