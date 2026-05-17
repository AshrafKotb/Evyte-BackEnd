using Eventa.ApplicationCore.Services.Repository;
using Eventa.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Eventa.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PredefinedTextsController : Controller
    {
        private readonly IPredefinedTextRepository _predefinedTextRepository;

        public PredefinedTextsController(IPredefinedTextRepository predefinedTextRepository)
        {
            _predefinedTextRepository = predefinedTextRepository;
        }

        public async Task<IActionResult> Index(string? textType = null)
        {
            var texts = await _predefinedTextRepository.GetAllAsync(textType);
            ViewBag.SelectedTextType = textType;
            return View(texts);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string textAr, string? textEn, string textType, int sortingNumber)
        {
            if (string.IsNullOrWhiteSpace(textAr))
            {
                ModelState.AddModelError("textAr", "النص بالعربي مطلوب");
                return View();
            }

            if (string.IsNullOrWhiteSpace(textType))
            {
                ModelState.AddModelError("textType", "نوع النص مطلوب");
                return View();
            }

            var predefinedText = new PredefinedText
            {
                TextAr = textAr,
                TextEn = textEn,
                TextType = textType,
                SortingNumber = sortingNumber
            };

            await _predefinedTextRepository.AddAsync(predefinedText);

            TempData["SuccessMessage"] = "تمت إضافة النص بنجاح";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var text = await _predefinedTextRepository.GetByIdAsync(id);
            if (text == null)
            {
                return NotFound();
            }
            return View(text);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, string textAr, string? textEn, string textType, int sortingNumber)
        {
            var text = await _predefinedTextRepository.GetByIdAsync(id);
            if (text == null)
            {
                return NotFound();
            }

            text.TextAr = textAr;
            text.TextEn = textEn;
            text.TextType = textType;
            text.SortingNumber = sortingNumber;

            await _predefinedTextRepository.UpdateAsync(text);

            TempData["SuccessMessage"] = "تم تعديل النص بنجاح";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _predefinedTextRepository.DeleteAsync(id);

            TempData["SuccessMessage"] = "تم حذف النص بنجاح";
            return RedirectToAction(nameof(Index));
        }
    }
}
