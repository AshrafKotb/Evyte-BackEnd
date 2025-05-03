using Evyte.ApplicationCore.Services.Files;
using Evyte.ApplicationCore.Services.Repository;
using Evyte.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Evyte.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RequestsController : Controller
    {
        private readonly IRequestRepository _requestRepository;
        private readonly IFileService _fileService;

        public RequestsController(IRequestRepository requestRepository, IFileService fileService)
        {
            _requestRepository = requestRepository;
            _fileService = fileService;
        }

        // GET: Requests
        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 10, string searchTerm = "")
        {
            var paginatedRequests = await _requestRepository.GetRequestsPaginatedAsync(pageNumber, pageSize, searchTerm);
            ViewBag.SearchTerm = searchTerm;
            ViewBag.PageSize = pageSize;
            return View(paginatedRequests);
        }

        // GET: Requests/Details/5
        public async Task<IActionResult> Details(Guid id)
        {
            var request = await _requestRepository.GetRequestByIdAsync(id);
            if (request == null)
            {
                return NotFound();
            }
            return View(request);
        }
    }
}