using Eventa.ApplicationCore.Services.Repository;
using Eventa.Domain.Entities;
using Eventa.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Eventa.Web.Controllers
{
    public class WeddingController : Controller
    {
        private readonly IRequestRepository _requestRepository;

        public WeddingController(IRequestRepository requestRepository)
        {
            _requestRepository = requestRepository;
        }



        [Route("e/{slug}")]
        [AllowAnonymous]
        public async Task<IActionResult> View(string slug)
        {
            if (string.IsNullOrEmpty(slug))
            {
                return NotFound();
            }

            var request = await _requestRepository.GetRequestBySlugAsync(slug);
            if (request == null)
            {
                return NotFound();
            }
            // ?????? ?? ???? ??????
            if (request.Status == InvitationStatus.Pending)
            {
                return View("InvitationPending", request);
            }
            else if (request.Status == InvitationStatus.Rejected)
            {
                return View("InvitationRejected", request);
            }

            // ?????? ?? ?? ????? ????? TimeSpan ?? ???? ?????
            if (request.RequestData.EventTimeFrom.TotalHours >= 24)
            {
                request.RequestData.EventTimeFrom = TimeSpan.FromHours(request.RequestData.EventTimeFrom.TotalHours % 24);
            }
            if (request.RequestData.EventTimeTo.TotalHours >= 24)
            {
                request.RequestData.EventTimeTo = TimeSpan.FromHours(request.RequestData.EventTimeTo.TotalHours % 24);
            }

            // ??? ??? Partial View ????? ??? TemplateName
            return View($"~/Views/Shared/templates/_{request.Design.TemplateName}.cshtml", request);
        }

    }
}