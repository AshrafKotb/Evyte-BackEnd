using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evyte.Domain.Entities
{
    public class RequestData : BaseEntity
    {

        #region Gromm Data
        public string GrommName { get; set; }
        public string GrommImageId { get; set; }
        public string GrommImageUrl { get; set; }
        public string GrommFacebook { get; set; }
        public string GrommInstagram { get; set; }
        public string GrommX { get; set; }
        #endregion

        #region Bride Data
        public string BrideName { get; set; }
        public string BrideImageId { get; set; }
        public string BrideImageUrl { get; set; }
        public string BrideFacebook { get; set; }
        public string BrideInstagram { get; set; }
        public string BrideX { get; set; }
        #endregion
        public string MainSliderImageId { get; set; }
        public string MainSliderImageUrl { get; set; }
        public DateOnly EventDate { get; set; }
        public TimeOnly EventTimeFrom { get; set; }
        public TimeOnly EventTimeTo { get; set; }

        #region EventPlace Data
        public string EventPlaceName { get; set; }
        public string EventPlaceImageId { get; set; }
        public string EventPlaceImageUrl { get; set; }
        public string EventAddress { get; set; }
        public string LocationUrl { get; set; }
        #endregion

    }
}
