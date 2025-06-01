using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evyte.Domain.Entities
{
    public class RequestData : BaseEntity
    {

        #region Groom Data
        public string GroomName { get; set; }
        public string? GroomImageId { get; set; }
        public string GroomImageUrl { get; set; }
        public string? GroomFacebook { get; set; }
        public string? GroomInstagram { get; set; }
        public string? GroomX { get; set; }
        #endregion

        #region Bride Data
        public string BrideName { get; set; }
        public string? BrideImageId { get; set; }
        public string BrideImageUrl { get; set; }
        public string? BrideFacebook { get; set; }
        public string? BrideInstagram { get; set; }
        public string? BrideX { get; set; }
        #endregion
        public string MainSliderImageId { get; set; }
        public string MainSliderImageUrl { get; set; }
        public DateTime EventDate { get; set; }
        public TimeSpan EventTimeFrom { get; set; }
        public TimeSpan EventTimeTo { get; set; }

        #region EventPlace Data
        public string EventPlaceName { get; set; }
        public string? EventPlaceImageId { get; set; }
        public string EventPlaceImageUrl { get; set; }
        public string EventAddress { get; set; }
        public string LocationUrl { get; set; }
        #endregion

    }
}
