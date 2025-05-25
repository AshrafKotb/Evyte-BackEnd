using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evyte.ApplicationCore.Models.ViewModels
{
    public class DesignViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PreviewImageUrl { get; set; }
        public string WebsiteDemoUrl { get; set; }
    }

    public class HomeIndexViewModel
    {
        public List<DesignViewModel> Designs { get; set; } = new List<DesignViewModel>();
    }
}
