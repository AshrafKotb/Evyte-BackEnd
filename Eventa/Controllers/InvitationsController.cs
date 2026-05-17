using Eventa.ApplicationCore.Interfaces.Services;
using Eventa.ApplicationCore.Models.ViewModels;
using Eventa.ApplicationCore.Services;
using Eventa.ApplicationCore.Services.Files;
using Eventa.ApplicationCore.Services.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Eventa.ApplicationCore.Controllers
{
    [AllowAnonymous]
    public class InvitationsController : Controller
    {
        private readonly IDesignService _designService;
        private readonly IInvitationService _invitationService;
        private readonly IBackgroundImageRepository _backgroundImageRepository;
        private readonly IPredefinedTextRepository _predefinedTextRepository;
        private readonly ISplashTemplateRepository _splashTemplateRepository;

        public InvitationsController(
            IDesignService designService,
            IInvitationService invitationService,
            IBackgroundImageRepository backgroundImageRepository,
            IPredefinedTextRepository predefinedTextRepository,
            ISplashTemplateRepository splashTemplateRepository)
        {
            _invitationService = invitationService;
            _designService = designService;
            _backgroundImageRepository = backgroundImageRepository;
            _predefinedTextRepository = predefinedTextRepository;
            _splashTemplateRepository = splashTemplateRepository;
        }

        // GET: /Invitations/CreateNew - New wizard-style form
        [HttpGet]
        public async Task<IActionResult> CreateNew(Guid? designId = null)
        {
            var designs = await _designService.GetAllDesignsAsync();
            ViewBag.Designs = designs;
            ViewBag.SelectedDesignId = designId;

            // Load background images library and predefined texts
            ViewBag.BackgroundImages = await _backgroundImageRepository.GetAllAsync();
            ViewBag.PredefinedTexts1 = await _predefinedTextRepository.GetAllAsync("sentence1");
            ViewBag.PredefinedTexts2 = await _predefinedTextRepository.GetAllAsync("sentence2");
            ViewBag.SplashTemplates = await _splashTemplateRepository.GetAllAsync(activeOnly: true);
            ViewBag.DefaultSplashTemplate = await _splashTemplateRepository.GetDefaultAsync();

            var model = new CreateInvitationVM();
            if (designId.HasValue && designId.Value != Guid.Empty)
            {
                model.DesignId = designId.Value;
            }

            return View(model);
        }

        // GET: /Invitations/Create?designId=GUID (legacy)
        [HttpGet]
        public async Task<IActionResult> Create(Guid designId)
        {
            if (designId == Guid.Empty)
                RedirectToAction("Index", "Home");

            var IsExist = _designService.IsExist(designId);
            if (!IsExist)
                return RedirectToAction("Index", "Home");

            // Load background images library and predefined texts
            ViewBag.BackgroundImages = await _backgroundImageRepository.GetAllAsync();
            ViewBag.PredefinedTexts1 = await _predefinedTextRepository.GetAllAsync("sentence1");
            ViewBag.PredefinedTexts2 = await _predefinedTextRepository.GetAllAsync("sentence2");
            ViewBag.SplashTemplates = await _splashTemplateRepository.GetAllAsync(activeOnly: true);
            ViewBag.DefaultSplashTemplate = await _splashTemplateRepository.GetDefaultAsync();

            var model = new CreateInvitationVM { DesignId = designId };
            return View(model);
        }

        // POST: /Invitations/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateInvitationVM dto)
        {
            if (dto.GroomImage != null)
            {
                ModelState.Remove("GroomAvatar");
            }
            if (dto.BrideImage != null)
            {
                ModelState.Remove("BrideAvatar");
            }

            // Remove validation for optional sections if not enabled
            if (!dto.HasGallery)
            {
                ModelState.Remove("GalleryPhotos");
            }

            if (!ModelState.IsValid)
            {
                var errors = ModelState.ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                );
                return Json(new { success = false, message = "Validation failed", errors });
            }

            try
            {
                var result = await _invitationService.CreateInvitationAsync(dto);
                return Json(new
                {
                    success = true,
                    invitationUrl = result.InvitationUrl,
                    qrCodeUrl = result.QrCodeUrl
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"An error occurred: {ex.Message}" });
            }
        }

    }
}