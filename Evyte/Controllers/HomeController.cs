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

    public HomeController(ILogger<HomeController> logger, IDesignService designService)
    {
        _logger = logger;
        _designService = designService;
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

        // إنشاء قائمة تحتوي على كل تصميم مكرر 10 مرات
        var repeatedDesigns = designs
            //.SelectMany(d => Enumerable.Repeat(d, 6)) // كرر كل عنصر 6 مرات
            .Select((d, index) => new DesignViewModel
            {
                Id = d.Id,
                NameAr = d.NameAr,
                DescriptionAr = d.DescriptionAr,
                PreviewImageUrl = d.ImageUrl,
                WebsiteDemoUrl = d.WebsiteDemoUrl
            }).ToList();

        var model = new HomeIndexViewModel
        {
            Designs = repeatedDesigns
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
                NameAr = d.NameAr,
                DescriptionAr = d.DescriptionAr,
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
