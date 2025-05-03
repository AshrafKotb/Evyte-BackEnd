using Microsoft.AspNetCore.Mvc.Rendering;

namespace Evyte.ApplicationCore.Models.ViewModels
{
    public class CustomerViewModel
    {
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public int RequestCount { get; set; }
        public IEnumerable<RequestQrCodeViewModel> Requests { get; set; }
    }

    public class RequestQrCodeViewModel
    {
        public string QrCodeImageUrl { get; set; }
        public string DomainUrl { get; set; }
    }
    public class CustomerIndexVM
    {
        public IEnumerable<CustomerViewModel> Customers { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public string SearchTerm { get; set; }
        public List<SelectListItem> PageSizeOptions { get; set; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "5", Text = "5" },
            new SelectListItem { Value = "10", Text = "10" },
            new SelectListItem { Value = "20", Text = "20" },
            new SelectListItem { Value = "50", Text = "50" }
        };
    }
}
