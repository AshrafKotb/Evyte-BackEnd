using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Evyte.ApplicationCore.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Evyte.ApplicationCore.Models.Helper;
using Evyte.ApplicationCore.Interfaces.Services;
using Evyte.ApplicationCore.Models.ViewModels.Categories;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Evyte.Controllers;
[Authorize(Roles = RoleName.Admin)]
public class CategoriesController : Controller
{
    private readonly ICategoryService _categoryService;
    private const int DefaultPageSize = 10;

    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    // GET: Categories
    public async Task<IActionResult> Index(int page = 1, int pageSize = DefaultPageSize, string searchTerm = "")
    {
        var model = await _categoryService.GetCategoriesPaginatedAsync(page, pageSize, searchTerm);
        var viewModel = new CategoryIndexVM
        {
            Categories = model.Items,
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

    // GET: Categories/Details/5
    public async Task<IActionResult> Details(Guid id)
    {
        var category = await _categoryService.GetCategoryByIdAsync(id);
        if (category == null)
        {
            return NotFound();
        }
        return View(category);
    }

    // GET: Categories/Create
    public IActionResult Create()
    {
        return View(new CreateCategoryVM() { });
    }

    // POST: Categories/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateCategoryVM model)
    {
        if (ModelState.IsValid)
        {
            var result = await _categoryService.CreateCategoryAsync(model);
            if (result != null)
            {
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError("", "Error creating category");
        }
        return View(model);
    }

    // GET: Categories/Edit/5
    public async Task<IActionResult> Edit(Guid id)
    {
        var category = await _categoryService.GetCategoryEntityByIdAsync(id);
        if (category == null)
        {
            return NotFound();
        }

        var model = new UpdateCategoryVM(category);

        return View(model);
    }

    // POST: Categories/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, UpdateCategoryVM model)
    {
        if (id != model.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            var result = await _categoryService.UpdateCategoryAsync(id, model);
            if (result != null)
            {
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError("", "Error updating category");
        }
        return View(model);
    }

    // POST: Categories/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var success = await _categoryService.DeleteCategoryAsync(id);
        if (!success)
        {
            return NotFound();
        }
        return RedirectToAction(nameof(Index));
    }

    // POST: Categories/Restore/5
    [HttpPost, ActionName("Restore")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RestoreConfirmed(Guid id)
    {
        var success = await _categoryService.RestoreCategoryAsync(id);
        if (!success)
        {
            return NotFound();
        }
        return RedirectToAction(nameof(Index));
    }
}