using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evyte.Domain.Entities
{
    public class Design : BaseEntity
    {
        public string NameAr { get; set; }
        public string NameEn { get; set; } = string.Empty;
        public string DescriptionAr { get; set; } = string.Empty;
        public string DescriptionEn { get; set; } = string.Empty;
        public string WebsiteDemoUrl { get; set; }
        public int SortingNumber { get; set; }
        public Guid CategoryId { get; set; }
        public DesignCategory Category { get; set; }
        public IEnumerable<Request> Requests { get; set; }
    }
}
