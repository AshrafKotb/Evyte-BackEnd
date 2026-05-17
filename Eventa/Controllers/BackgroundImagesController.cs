using Eventa.ApplicationCore.Services.Files;
using Eventa.ApplicationCore.Services.Repository;
using Eventa.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Eventa.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class BackgroundImagesController : Controller
    {
        private readonly IBackgroundImageRepository _backgroundImageRepository;
        private readonly IFileService _fileService;

        public BackgroundImagesController(
            IBackgroundImageRepository backgroundImageRepository,
            IFileService fileService)
        {
            _backgroundImageRepository = backgroundImageRepository;
            _fileService = fileService;
        }

        public async Task<IActionResult> Index(string? category = null)
        {
            var images = await _backgroundImageRepository.GetAllAsync(category);
            ViewBag.SelectedCategory = category;
            return View(images);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string? nameAr, string? nameEn, string? category, int sortingNumber, IFormFile imageFile)
        {
            if (imageFile == null)
            {
                ModelState.AddModelError("imageFile", "الصورة مطلوبة");
                return View();
            }

            var (url, id) = await _fileService.UploadPictureAsync(imageFile, "backgrounds");

            var backgroundImage = new BackgroundImage
            {
                ImageUrl = url,
                ImageId = id,
                NameAr = nameAr,
                NameEn = nameEn,
                Category = category,
                SortingNumber = sortingNumber
            };

            await _backgroundImageRepository.AddAsync(backgroundImage);

            TempData["SuccessMessage"] = "تمت إضافة الصورة بنجاح";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            var image = await _backgroundImageRepository.GetByIdAsync(id);
            if (image == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(image.ImageId))
            {
                await _fileService.DeletePictureAsync(image.ImageId);
            }

            await _backgroundImageRepository.DeleteAsync(id);

            TempData["SuccessMessage"] = "تم حذف الصورة بنجاح";
            return RedirectToAction(nameof(Index));
        }
    }
}
