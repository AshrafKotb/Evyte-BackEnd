using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evyte.Domain.Entities
{
    public class Request : BaseEntity
    {
        public string DomainUrl { get; set; }
        public string QrCodeImageUrl { get; set; }
        public string QrCodeImageId { get; set; }
        public Guid RequestDataId { get; set; }
        public RequestData RequestData { get; set; }
        public Guid DesignId { get; set; }
        public Design Design { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public IEnumerable<RequestGalleryPhoto> GalleryPhotos { get; set; }

    }
}
