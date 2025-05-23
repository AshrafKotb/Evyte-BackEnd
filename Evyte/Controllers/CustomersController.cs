using Evyte.ApplicationCore.Models.Helper;
using Evyte.ApplicationCore.Models.ViewModels;
using Evyte.ApplicationCore.Services.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace Evyte.Controllers
{
    [Authorize(Roles = RoleName.Admin)]
    public class CustomersController : Controller
    {
        private readonly IUserRepository _customerRepository;

        public CustomersController(IUserRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<IActionResult> Index(int page = 1, int pageSize = 10, string searchTerm = "")
        {
            var paginatedResult = await _customerRepository.GetAllCustomersWithRequestsAsync(page, pageSize, searchTerm);

            var viewModel = new CustomerIndexVM
            {
                Customers = paginatedResult.Items,
                CurrentPage = paginatedResult.CurrentPage,
                PageSize = paginatedResult.PageSize,
                TotalItems = paginatedResult.TotalCount,
                SearchTerm = searchTerm,
                PageSizeOptions = new List<SelectListItem>
                {
                    new SelectListItem { Value = "5", Text = "5" },
                    new SelectListItem { Value = "10", Text = "10" },
                    new SelectListItem { Value = "20", Text = "20" },
                    new SelectListItem { Value = "50", Text = "50" }
                }
            };

            return View(viewModel);
        }
    }
}