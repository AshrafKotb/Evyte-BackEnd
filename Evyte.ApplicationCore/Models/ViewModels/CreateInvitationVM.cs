using Microsoft.AspNetCore.Http;

namespace Evyte.ApplicationCore.Models.ViewModels
{
    public class CreateInvitationVM
    {
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string GroomName { get; set; }
        public IFormFile GroomImage { get; set; }
        public string GroomFacebook { get; set; }
        public string GroomInstagram { get; set; }
        public string GroomX { get; set; }
        public string BrideName { get; set; }
        public IFormFile BrideImage { get; set; }
        public string BrideFacebook { get; set; }
        public string BrideInstagram { get; set; }
        public string BrideX { get; set; }
        public IFormFile MainSliderImage { get; set; }
        public DateOnly EventDate { get; set; }
        public TimeOnly EventTimeFrom { get; set; }
        public TimeOnly EventTimeTo { get; set; }
        public string EventPlaceName { get; set; }
        public IFormFile EventPlaceImage { get; set; }
        public string EventAddress { get; set; }
        public string LocationUrl { get; set; }
        public IFormFileCollection GalleryPhotos { get; set; }
        public Guid DesignId { get; set; }
    }

}
