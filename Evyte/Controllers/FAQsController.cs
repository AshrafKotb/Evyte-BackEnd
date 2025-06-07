using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Evyte.ApplicationCore.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Evyte.ApplicationCore.Models.Helper;
using Evyte.ApplicationCore.Interfaces.Services;
using Evyte.ApplicationCore.Models.ViewModels.FAQs;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Evyte.Controllers;
[Authorize(Roles = RoleName.Admin)]
public class FAQsController : Controller
{
    private readonly IFAQService _FAQService;
    private const int DefaultPageSize = 10;

    public FAQsController(IFAQService FAQService)
    {
        _FAQService = FAQService;
    }

    // GET: FAQs
    public async Task<IActionResult> Index(int page = 1, int pageSize = DefaultPageSize, string searchTerm = "")
    {
        var model = await _FAQService.GetFAQsPaginatedAsync(page, pageSize, searchTerm);
        var viewModel = new FAQIndexVM
        {
            FAQs = model.Items,
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

    // GET: FAQs/Details/5
    public async Task<IActionResult> Details(Guid id)
    {
        var FAQ = await _FAQService.GetFAQByIdAsync(id);
        if (FAQ == null)
        {
            return NotFound();
        }
        return View(FAQ);
    }

    // GET: FAQs/Create
    public IActionResult Create()
    {
        return View(new CreateFAQVM() { });
    }

    // POST: FAQs/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateFAQVM model)
    {
        if (ModelState.IsValid)
        {
            var result = await _FAQService.CreateFAQAsync(model);
            if (result != null)
            {
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError("", "Error creating FAQ");
        }
        return View(model);
    }

    // GET: FAQs/Edit/5
    public async Task<IActionResult> Edit(Guid id)
    {
        var FAQ = await _FAQService.GetFAQEntityByIdAsync(id);
        if (FAQ == null)
        {
            return NotFound();
        }

        var model = new UpdateFAQVM(FAQ);

        return View(model);
    }

    // POST: FAQs/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, UpdateFAQVM model)
    {
        if (id != model.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            var result = await _FAQService.UpdateFAQAsync(id, model);
            if (result != null)
            {
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError("", "Error updating FAQ");
        }
        return View(model);
    }

    // POST: FAQs/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var success = await _FAQService.DeleteFAQAsync(id);
        if (!success)
        {
            return NotFound();
        }
        return RedirectToAction(nameof(Index));
    }

    // POST: FAQs/Restore/5
    [HttpPost, ActionName("Restore")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RestoreConfirmed(Guid id)
    {
        var success = await _FAQService.RestoreFAQAsync(id);
        if (!success)
        {
            return NotFound();
        }
        return RedirectToAction(nameof(Index));
    }
}