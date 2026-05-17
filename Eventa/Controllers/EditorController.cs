using Eventa.ApplicationCore.Interfaces.Services;
using Eventa.ApplicationCore.Models.ViewModels;
using Eventa.ApplicationCore.Services.Files;
using Eventa.ApplicationCore.Services.Repository;
using Eventa.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Eventa.Web.Controllers
{
    [AllowAnonymous]
    public class EditorController : Controller
    {
        private readonly IDesignService _designService;
        private readonly IFileService _fileService;
        private readonly IInvitationService _invitationService;
        private readonly IPredefinedTextRepository _predefinedTextRepository;
        private readonly IBackgroundImageRepository _backgroundImageRepository;

        public EditorController(
            IDesignService designService,
            IFileService fileService,
            IInvitationService invitationService,
            IPredefinedTextRepository predefinedTextRepository,
            IBackgroundImageRepository backgroundImageRepository)
        {
            _designService = designService;
            _fileService = fileService;
            _invitationService = invitationService;
            _predefinedTextRepository = predefinedTextRepository;
            _backgroundImageRepository = backgroundImageRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Design(Guid id)
        {
            var design = await _designService.GetDesignEntityByIdAsync(id);
            if (design == null)
            {
                return NotFound();
            }

            // Build a placeholder Request to render the template
            var placeholderRequest = new Request
            {
                DesignId = design.Id,
                Design = design,
                RequestData = new RequestData
                {
                    GroomName = "اسم العريس",
                    BrideName = "اسم العروسة",
                    GroomImageUrl = "https://ik.imagekit.io/Ashraf/Avatar/groom1.jpg",
                    BrideImageUrl = "https://ik.imagekit.io/Ashraf/Avatar/bride1.jpg",
                    MainSliderImageUrl = "https://ik.imagekit.io/Ashraf/eventplace/cover2.jpg",
                    EventDate = DateTime.Now.AddMonths(3),
                    EventTimeFrom = new TimeSpan(18, 0, 0),
                    EventTimeTo = new TimeSpan(23, 0, 0),
                    EventPlaceName = "اسم قاعة الأفراح",
                    EventAddress = "العنوان",
                    LocationUrl = "",
                    EventPlaceImageUrl = "https://ik.imagekit.io/Ashraf/eventplace/cover3.jpg"
                },
                HasGallery = true,
                HasMemories = true,
                HasDedications = true,
                HasAudio = false,
                GalleryPhotos = new List<RequestGalleryPhoto>
                {
                    new RequestGalleryPhoto { PhotoUrl = "https://ik.imagekit.io/Ashraf/eventplace/cover2.jpg", PhotoId = "placeholder" },
                    new RequestGalleryPhoto { PhotoUrl = "https://ik.imagekit.io/Ashraf/eventplace/cover3.jpg", PhotoId = "placeholder" },
                },
                Memories = new List<Memory>
                {
                    new Memory { Title = "الخطوبة", EventDate = DateTime.Now.AddMonths(-6), ImageUrl = "https://ik.imagekit.io/Ashraf/eventplace/cover2.jpg", DisplayOrder = 0 }
                },
                Dedications = new List<Dedication>
                {
                    new Dedication { PersonName = "اسم الشخص", Relationship = "صلة القرابة", Message = "نص الإهداء", DisplayOrder = 0 }
                }
            };

            // Load predefined texts and background images for the editor panel
            var predefinedTexts1 = await _predefinedTextRepository.GetAllAsync("sentence1");
            var predefinedTexts2 = await _predefinedTextRepository.GetAllAsync("sentence2");
            var backgroundImages = await _backgroundImageRepository.GetAllAsync();

            ViewBag.IsEditorMode = true;
            ViewBag.DesignId = design.Id;
            ViewBag.DesignName = design.NameAr;
            ViewBag.PredefinedTexts1 = predefinedTexts1;
            ViewBag.PredefinedTexts2 = predefinedTexts2;
            ViewBag.BackgroundImages = backgroundImages;

            return View("~/Views/Editor/Design.cshtml", placeholderRequest);
        }

        /// <summary>
        /// Renders the template as a standalone page for the Editor iframe.
        /// Same placeholder data as Design(), but returns only the template partial (full HTML page).
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> TemplatePreview(Guid id)
        {
            var design = await _designService.GetDesignEntityByIdAsync(id);
            if (design == null)
                return NotFound();

            var placeholderRequest = new Request
            {
                DesignId = design.Id,
                Design = design,
                RequestData = new RequestData
                {
                    GroomName = "اسم العريس",
                    BrideName = "اسم العروسة",
                    GroomImageUrl = "https://ik.imagekit.io/Ashraf/Avatar/groom1.jpg",
                    BrideImageUrl = "https://ik.imagekit.io/Ashraf/Avatar/bride1.jpg",
                    MainSliderImageUrl = "https://ik.imagekit.io/Ashraf/eventplace/cover2.jpg",
                    EventDate = DateTime.Now.AddMonths(3),
                    EventTimeFrom = new TimeSpan(18, 0, 0),
                    EventTimeTo = new TimeSpan(23, 0, 0),
                    EventPlaceName = "اسم قاعة الأفراح",
                    EventAddress = "العنوان",
                    LocationUrl = "",
                    EventPlaceImageUrl = "https://ik.imagekit.io/Ashraf/eventplace/cover3.jpg"
                },
                HasGallery = true,
                HasMemories = true,
                HasDedications = true,
                HasAudio = false,
                GalleryPhotos = new List<RequestGalleryPhoto>
                {
                    new RequestGalleryPhoto { PhotoUrl = "https://ik.imagekit.io/Ashraf/eventplace/cover2.jpg", PhotoId = "placeholder" },
                    new RequestGalleryPhoto { PhotoUrl = "https://ik.imagekit.io/Ashraf/eventplace/cover3.jpg", PhotoId = "placeholder" },
                },
                Memories = new List<Memory>
                {
                    new Memory { Title = "الخطوبة", EventDate = DateTime.Now.AddMonths(-6), ImageUrl = "https://ik.imagekit.io/Ashraf/eventplace/cover2.jpg", DisplayOrder = 0 }
                },
                Dedications = new List<Dedication>
                {
                    new Dedication { PersonName = "اسم الشخص", Relationship = "صلة القرابة", Message = "نص الإهداء", DisplayOrder = 0 }
                }
            };

            ViewBag.IsEditorMode = true;

            // Return the template as a full standalone page
            return View($"~/Views/Shared/templates/_{design.TemplateName}.cshtml", placeholderRequest);
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        [RequestFormLimits(MultipartBodyLengthLimit = 20_000_000)] // 20 MB
        [RequestSizeLimit(20_000_000)]
        public async Task<IActionResult> UploadImage(IFormFile file, string folder = "editor")
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return Json(new { success = false, message = "لم يتم تحديد ملف" });
                }

                var (url, id) = await _fileService.UploadPictureAsync(file, folder);
                return Json(new { success = true, url, id });
            }
            catch (Exception ex)
            {
                var message = ex.InnerException?.Message ?? ex.Message;
                if (message.Contains("فشل رفع الصورة")) return Json(new { success = false, message = message });
                return Json(new { success = false, message = "فشل رفع الملف. تأكد من اتصال الإنترنت وحجم الصورة ثم أعد المحاولة." });
            }
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        [RequestFormLimits(MultipartBodyLengthLimit = 20_000_000)]
        [RequestSizeLimit(20_000_000)]
        public async Task<IActionResult> UploadAudio(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return Json(new { success = false, message = "لم يتم تحديد ملف" });
                }

                var (url, id) = await _fileService.UploadFileAsync(file, "audio");
                return Json(new { success = true, url, id });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "فشل رفع الملف الصوتي" });
            }
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> Submit([FromBody] EditorSubmitDTO dto)
        {
            if (dto == null)
            {
                return Json(new { success = false, message = "البيانات غير صحيحة" });
            }

            if (string.IsNullOrWhiteSpace(dto.GroomName) || string.IsNullOrWhiteSpace(dto.BrideName))
            {
                return Json(new { success = false, message = "اسم العريس والعروسة مطلوبان" });
            }

            if (string.IsNullOrWhiteSpace(dto.FullName) || string.IsNullOrWhiteSpace(dto.PhoneNumber) || string.IsNullOrWhiteSpace(dto.Email))
            {
                return Json(new { success = false, message = "بيانات التواصل مطلوبة" });
            }

            try
            {
                var (invitationUrl, qrCodeUrl) = await _invitationService.CreateFromEditorAsync(dto);
                return Json(new { success = true, invitationUrl, qrCodeUrl, message = "تم إرسال طلبك بنجاح! سيتم مراجعته والموافقة عليه قريباً." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "حدث خطأ أثناء حفظ الطلب. يرجى المحاولة مرة أخرى." });
            }
        }
    }
}
