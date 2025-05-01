using Evyte.ApplicationCore.Models.ViewModels;
using Evyte.ApplicationCore.Services;
using Evyte.ApplicationCore.Services.Files;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Evyte.ApplicationCore.Controllers
{
    [AllowAnonymous]
    public class InvitationController : Controller
    {
        private readonly IInvitationService _invitationService;

        public InvitationController(IInvitationService invitationService)
        {
            _invitationService = invitationService;
        }

        // GET: /Invitations/Create?designId=GUID
        [HttpGet]
        public IActionResult Create(Guid designId)
        {
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
                return View(dto);
            }

            try
            {
                // Call the service to process the invitation
                var result = await _invitationService.CreateInvitationAsync(dto);
                ViewData["Result"] = new
                {
                    InvitationUrl = result.InvitationUrl,
                    QrCodeUrl = result.QrCodeUrl
                };
                return View(dto);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"An error occurred: {ex.Message}");
                return View(dto);
            }
        }
    }
}