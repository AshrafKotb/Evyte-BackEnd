using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Evyte.ApplicationCore.Models.Helper;
using Evyte.ApplicationCore.Interfaces.Services;
using Evyte.ApplicationCore.Models.ViewModels.Designs;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Threading.Tasks;
using System.Linq;
using Evyte.Infrastructure;
using Evyte.Domain.Entities;

namespace Evyte.Controllers
{
    [Authorize(Roles = RoleName.Admin)]
    public class DesignsController : Controller
    {
        private readonly IDesignService _designService;
        private readonly ICategoryService _categoryService;
        private const int DefaultPageSize = 10;

        public DesignsController(IDesignService designService, ICategoryService categoryService)
        {
            _designService = designService;
            _categoryService = categoryService;
        }

        // GET: Designs
        public async Task<IActionResult> Index(int page = 1, int pageSize = DefaultPageSize, string searchTerm = "")
        {
            var model = await _designService.GetDesignsPaginatedAsync(page, pageSize, searchTerm);
            var viewModel = new DesignIndexVM
            {
                Designs = model.Items,
                CurrentPage = page,
                PageSize = pageSize,
                TotalItems = model.TotalCount,
                SearchTerm = searchTerm,
                PageSizeOptions = new List<SelectListItem>
                {
                    new SelectListItem { Value = "5", Text = "5" },
                    new SelectListItem { Value = "10", Text = "10" },
                    new SelectListItem { Value = "20", Text = "20" },
                    new SelectListItem { Value = "50", Text = "50" }
                }
            };

            return View(viewModel);
        }

        // GET: Designs/Details/5
        public async Task<IActionResult> Details(Guid id)
        {
            var design = await _designService.GetDesignByIdAsync(id);
            if (design == null)
            {
                return NotFound();
            }
            return View(design);
        }

        // GET: Designs/Create
        public async Task<IActionResult> Create()
        {
            var categories = await _categoryService.GetCategoriesPaginatedAsync(1, int.MaxValue);
            ViewBag.Categories = new SelectList(categories.Items, "Id", "NameAr");
            return View(new CreateDesignVM());
        }

        // POST: Designs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateDesignVM model)
        {
            if (ModelState.IsValid)
            {
                var result = await _designService.CreateDesignAsync(model);
                if (result != null)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Error creating design");
            }

            var categories = await _categoryService.GetCategoriesPaginatedAsync(1, int.MaxValue);
            ViewBag.Categories = new SelectList(categories.Items, "Id", "NameAr", model.CategoryId);
            return View(model);
        }

        // GET: Designs/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {
            var design = await _designService.GetDesignEntityByIdAsync(id);
            if (design == null)
            {
                return NotFound();
            }

            var model = new UpdateDesignVM(design);
            var categories = await _categoryService.GetCategoriesPaginatedAsync(1, int.MaxValue);
            ViewBag.Categories = new SelectList(categories.Items, "Id", "NameAr", design.CategoryId);

            return View(model);
        }

        // POST: Designs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, UpdateDesignVM model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var result = await _designService.UpdateDesignAsync(id, model);
                if (result != null)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Error updating design");
            }

            var categories = await _categoryService.GetCategoriesPaginatedAsync(1, int.MaxValue);
            ViewBag.Categories = new SelectList(categories.Items, "Id", "NameAr", model.CategoryId);
            return View(model);
        }

        // POST: Designs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var success = await _designService.DeleteDesignAsync(id);
            if (!success)
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Index));
        }

        // POST: Designs/Restore/5
        [HttpPost, ActionName("Restore")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RestoreConfirmed(Guid id)
        {
            var success = await _designService.RestoreDesignAsync(id);
            if (!success)
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Index));
        }


        [Route("/design/preview/{templateName}")]
        [AllowAnonymous]
        public async Task<IActionResult> Preview(string templateName)
        {
            var design = await _designService.GetDesignByTemplateNameAsync(templateName);
            if (design == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var dummyRequest = new Request
            {
                WeddingSlug = "preview",
                Design = design,
                RequestData = new RequestData
                {
                    GroomName = "عمر",
                    BrideName = "لبني",
                    EventDate = DateTime.Now.AddDays(30),
                    EventTimeFrom = TimeSpan.FromHours(18),
                    EventTimeTo = TimeSpan.FromHours(23),
                    EventPlaceName = "قاعة افراح الملكة",
                    EventAddress = "الغردقه",
                    MainSliderImageUrl = "https://ik.imagekit.io/Ashraf/slider/7ec54959-f31a-4521-961f-9dc55a3eb179?updatedAt=1746230565127",
                    EventPlaceImageUrl = "https://ik.imagekit.io/Ashraf/eventplace/cover2.jpg",
                    LocationUrl = "https://maps.app.goo.gl/TzDP3ivPfHLD2oGH9",
                    GroomImageUrl = "https://ik.imagekit.io/Ashraf/groom/groom1.jpeg?updatedAt=1747490608805",
                    BrideImageUrl = "https://ik.imagekit.io/Ashraf/bride/wedding-bride-girl.jpg?updatedAt=1747490553095"
                },
                GalleryPhotos = new List<RequestGalleryPhoto>
                {
                    new RequestGalleryPhoto { PhotoUrl = "https://ik.imagekit.io/Ashraf/gallery/8f9f9886-f11c-4573-9f3a-d0cdd924902e?updatedAt=1746235167623" },
                    new RequestGalleryPhoto { PhotoUrl = "https://ik.imagekit.io/Ashraf/gallery/16e59e13-7b10-49c8-83ef-a86f4621aaec?updatedAt=1746253016740" },
                    new RequestGalleryPhoto { PhotoUrl = "https://ik.imagekit.io/Ashraf/gallery/efef2c2d-3d4b-4001-9f07-a7701e9b86cf?updatedAt=1746268323967" },
                    new RequestGalleryPhoto { PhotoUrl = "https://ik.imagekit.io/Ashraf/gallery/photo5.jpg" },
                    new RequestGalleryPhoto { PhotoUrl = "https://ik.imagekit.io/Ashraf/gallery/photo1.png" },
                    new RequestGalleryPhoto { PhotoUrl = "https://ik.imagekit.io/Ashraf/gallery/cover7.jpg" },
                    new RequestGalleryPhoto { PhotoUrl = "https://ik.imagekit.io/Ashraf/gallery/photo2.jpg" }
                }
            };

            return View($"~/Views/Shared/templates/_{design.TemplateName}.cshtml", dummyRequest);
        }

    }
}