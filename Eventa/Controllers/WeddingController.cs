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
                Response.StatusCode = 404;
                return base.View("InvitationNotFound");
            }

            var request = await _requestRepository.GetRequestBySlugAsync(slug);
            if (request == null)
            {
                Response.StatusCode = 404;
                return base.View("InvitationNotFound");
            }
            // دعوة تحت المراجعة
            if (request.Status == InvitationStatus.Pending)
            {
                return base.View("InvitationPending", request);
            }
            else if (request.Status == InvitationStatus.Rejected)
            {
                return base.View("InvitationRejected", request);
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

            return base.View($"~/Views/Shared/templates/_{request.Design.TemplateName}.cshtml", request);
        }

    }
}