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
        private readonly IInvitationService _invitationService;

        public InvitationsController(IInvitationService invitationService)
        {
            _invitationService = invitationService;
        }

        // GET: /Invitations/Create?designId=GUID
        [HttpGet]
        public IActionResult Create(Guid designId)
        {
            if (designId == Guid.Empty)
            {
                designId = new Guid("c87af62b-f412-4c4c-8382-ecca50d3d2d7");
            }
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