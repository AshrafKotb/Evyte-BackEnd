using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Evyte.ApplicationCore.Models.ViewModels.Designs
{
    public class DesignIndexVM
    {
        public IEnumerable<DesignVM> Designs { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public string SearchTerm { get; set; }
        public List<SelectListItem> PageSizeOptions { get; set; }

        public int TotalPages => (int)System.Math.Ceiling((double)TotalItems / PageSize);
    }
}