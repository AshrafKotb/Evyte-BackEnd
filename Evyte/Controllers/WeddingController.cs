using Evyte.ApplicationCore.Services.Repository;
using Evyte.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Evyte.Web.Controllers
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

            // التأكد من إن القيم بتاعة TimeSpan في نطاق مقبول
            if (request.RequestData.EventTimeFrom.TotalHours >= 24)
            {
                request.RequestData.EventTimeFrom = TimeSpan.FromHours(request.RequestData.EventTimeFrom.TotalHours % 24);
            }
            if (request.RequestData.EventTimeTo.TotalHours >= 24)
            {
                request.RequestData.EventTimeTo = TimeSpan.FromHours(request.RequestData.EventTimeTo.TotalHours % 24);
            }

            // عرض الـ Partial View بناءً على TemplateName
            return View($"~/Views/Shared/templates/_{request.Design.TemplateName}.cshtml", request);
        }

    }
}