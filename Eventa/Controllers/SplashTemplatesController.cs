using Eventa.ApplicationCore.Services.Files;
using Eventa.ApplicationCore.Services.Repository;
using Eventa.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Eventa.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SplashTemplatesController : Controller
    {
        // معاينة السبلاش كصفحة كاملة - متاحة للجميع (العملاء يستخدموها وقت اختيار السبلاش)
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Preview(Guid id, string? groomName = null, string? brideName = null)
        {
            var splash = await _splashRepository.GetByIdAsync(id);
            if (splash == null || !splash.IsActive)
                return NotFound();

            // بنبني Request وهمي عشان السبلاش الـ partial يلاقي بيانات يستخدمها (لو احتاج)
            var placeholder = new Request
            {
                RequestData = new RequestData
                {
                    GroomName = string.IsNullOrWhiteSpace(groomName) ? "محمد" : groomName,
                    BrideName = string.IsNullOrWhiteSpace(brideName) ? "سارة" : brideName,
                    EventDate = DateTime.Now.AddMonths(3),
                    EventPlaceName = "—",
                    EventAddress = "—"
                }
            };

            ViewBag.SplashTemplate = splash;
            ViewBag.SplashRequest = placeholder;
            return View(splash);
        }

        private readonly ISplashTemplateRepository _splashRepository;
        private readonly IFileService _fileService;

        public SplashTemplatesController(
            ISplashTemplateRepository splashRepository,
            IFileService fileService)
        {
            _splashRepository = splashRepository;
            _fileService = fileService;
        }

        public async Task<IActionResult> Index()
        {
            var splashes = await _splashRepository.GetAllAsync(activeOnly: false);
            return View(splashes);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            string nameAr,
            string nameEn,
            string partialName,
            string? descriptionAr,
            string? descriptionEn,
            int durationMs,
            bool requiresInteraction,
            bool isActive,
            bool isDefault,
            int sortingNumber,
            IFormFile? thumbnailFile)
        {
            if (string.IsNullOrWhiteSpace(nameAr))
                ModelState.AddModelError("nameAr", "الاسم بالعربي مطلوب");
            if (string.IsNullOrWhiteSpace(nameEn))
                ModelState.AddModelError("nameEn", "الاسم بالإنجليزي مطلوب");
            if (string.IsNullOrWhiteSpace(partialName))
                ModelState.AddModelError("partialName", "اسم ملف الـ Partial مطلوب");

            if (!ModelState.IsValid)
                return View();

            var splash = new SplashTemplate
            {
                NameAr = nameAr.Trim(),
                NameEn = nameEn.Trim(),
                PartialName = partialName.Trim(),
                DescriptionAr = descriptionAr,
                DescriptionEn = descriptionEn,
                DurationMs = durationMs > 0 ? durationMs : 3000,
                RequiresInteraction = requiresInteraction,
                IsActive = isActive,
                IsDefault = isDefault,
                SortingNumber = sortingNumber
            };

            if (thumbnailFile != null)
            {
                var (url, id) = await _fileService.UploadPictureAsync(thumbnailFile, "splash-thumbnails");
                splash.ThumbnailUrl = url;
                splash.ThumbnailId = id;
            }

            await _splashRepository.AddAsync(splash);

            TempData["SuccessMessage"] = "تمت إضافة السبلاش بنجاح";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var splash = await _splashRepository.GetByIdAsync(id);
            if (splash == null)
                return NotFound();
            return View(splash);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            Guid id,
            string nameAr,
            string nameEn,
            string partialName,
            string? descriptionAr,
            string? descriptionEn,
            int durationMs,
            bool requiresInteraction,
            bool isActive,
            bool isDefault,
            int sortingNumber,
            IFormFile? thumbnailFile)
        {
            var splash = await _splashRepository.GetByIdAsync(id);
            if (splash == null)
                return NotFound();

            if (string.IsNullOrWhiteSpace(nameAr))
                ModelState.AddModelError("nameAr", "الاسم بالعربي مطلوب");
            if (string.IsNullOrWhiteSpace(nameEn))
                ModelState.AddModelError("nameEn", "الاسم بالإنجليزي مطلوب");
            if (string.IsNullOrWhiteSpace(partialName))
                ModelState.AddModelError("partialName", "اسم ملف الـ Partial مطلوب");

            if (!ModelState.IsValid)
                return View(splash);

            splash.NameAr = nameAr.Trim();
            splash.NameEn = nameEn.Trim();
            splash.PartialName = partialName.Trim();
            splash.DescriptionAr = descriptionAr;
            splash.DescriptionEn = descriptionEn;
            splash.DurationMs = durationMs > 0 ? durationMs : 3000;
            splash.RequiresInteraction = requiresInteraction;
            splash.IsActive = isActive;
            splash.IsDefault = isDefault;
            splash.SortingNumber = sortingNumber;

            if (thumbnailFile != null)
            {
                if (!string.IsNullOrEmpty(splash.ThumbnailId))
                {
                    await _fileService.DeletePictureAsync(splash.ThumbnailId);
                }
                var (url, fileId) = await _fileService.UploadPictureAsync(thumbnailFile, "splash-thumbnails");
                splash.ThumbnailUrl = url;
                splash.ThumbnailId = fileId;
            }

            await _splashRepository.UpdateAsync(splash);

            TempData["SuccessMessage"] = "تم تعديل السبلاش بنجاح";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            var splash = await _splashRepository.GetByIdAsync(id);
            if (splash == null)
                return NotFound();

            if (splash.IsDefault)
            {
                TempData["ErrorMessage"] = "لا يمكن حذف السبلاش الافتراضي. يرجى تحديد سبلاش افتراضي آخر أولاً.";
                return RedirectToAction(nameof(Index));
            }

            if (!string.IsNullOrEmpty(splash.ThumbnailId))
            {
                await _fileService.DeletePictureAsync(splash.ThumbnailId);
            }

            await _splashRepository.DeleteAsync(id);

            TempData["SuccessMessage"] = "تم حذف السبلاش بنجاح";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleActive(Guid id)
        {
            var splash = await _splashRepository.GetByIdAsync(id);
            if (splash == null)
                return NotFound();

            if (splash.IsActive && splash.IsDefault)
            {
                TempData["ErrorMessage"] = "لا يمكن تعطيل السبلاش الافتراضي. يرجى تحديد سبلاش افتراضي آخر أولاً.";
                return RedirectToAction(nameof(Index));
            }

            var newState = await _splashRepository.ToggleActiveAsync(id);
            TempData["SuccessMessage"] = newState ? "تم تفعيل السبلاش" : "تم تعطيل السبلاش";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetDefault(Guid id)
        {
            var splash = await _splashRepository.GetByIdAsync(id);
            if (splash == null)
                return NotFound();

            if (!splash.IsActive)
            {
                TempData["ErrorMessage"] = "لا يمكن تعيين سبلاش غير مُفعّل كافتراضي. فعّله أولاً.";
                return RedirectToAction(nameof(Index));
            }

            await _splashRepository.SetAsDefaultAsync(id);
            TempData["SuccessMessage"] = "تم تعيين السبلاش كافتراضي";
            return RedirectToAction(nameof(Index));
        }
    }
}
