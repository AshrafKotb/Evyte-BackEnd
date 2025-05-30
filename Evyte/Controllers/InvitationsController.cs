using Evyte.ApplicationCore.Interfaces.Services;
using Evyte.ApplicationCore.Models.ViewModels;
using Evyte.ApplicationCore.Services;
using Evyte.ApplicationCore.Services.Files;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Evyte.ApplicationCore.Controllers
{
    [AllowAnonymous]
    public class InvitationsController : Controller
    {
        private readonly IDesignService _designService;
        private readonly IInvitationService _invitationService;

        public InvitationsController(IDesignService designService, IInvitationService invitationService)
        {
            _invitationService = invitationService;
            _designService = designService;
        }

        // GET: /Invitations/Create?designId=GUID
        [HttpGet]
        public IActionResult Create(Guid designId)
        {
            if (designId == Guid.Empty)
                RedirectToAction("Index", "Home");

            var IsExist = _designService.IsExist(designId);
            if (!IsExist)
                return RedirectToAction("Index", "Home");

            var model = new CreateInvitationVM { DesignId = designId };
            return View(model);
        }
        // POST: /Invitations/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateInvitationVM dto)
        {

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