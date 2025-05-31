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
            var categories = await _categoryService.GetAllActiveCategoriesAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "NameAr");
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

            var categories = await _categoryService.GetAllActiveCategoriesAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "NameAr", model.CategoryId);
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
            var categories = await _categoryService.GetAllActiveCategoriesAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "NameAr", design.CategoryId);

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

            var categories = await _categoryService.GetAllActiveCategoriesAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "NameAr", model.CategoryId);
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

            ViewBag.Id = design.Id.ToString();
            return View($"~/Views/Shared/Preview/_{design.TemplateName}.cshtml");
        }

    }
}