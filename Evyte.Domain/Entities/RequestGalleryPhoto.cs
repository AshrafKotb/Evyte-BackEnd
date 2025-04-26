using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evyte.Domain.Entities
{
    public class RequestGalleryPhoto : BaseEntity
    {
        public string PhotoId { get; set; }
        public string PhotoUrl { get; set; }
        public Guid RequestId { get; set; }
        public Request Request { get; set; }
    }
}
