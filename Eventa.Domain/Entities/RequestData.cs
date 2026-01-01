using System.ComponentModel.DataAnnotations;

namespace Eventa.Domain.Entities
{
    public class RequestData : BaseEntity
    {
        #region Groom Data
        [Required, MaxLength(200)]
        public string GroomName { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? GroomImageId { get; set; }

        [MaxLength(500)]
        public string? GroomImageUrl { get; set; }

        [MaxLength(500)]
        public string? GroomFacebook { get; set; }

        [MaxLength(500)]
        public string? GroomInstagram { get; set; }

        [MaxLength(500)]
        public string? GroomX { get; set; }
        #endregion

        #region Bride Data
        [Required, MaxLength(200)]
        public string BrideName { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? BrideImageId { get; set; }

        [MaxLength(500)]
        public string? BrideImageUrl { get; set; }

        [MaxLength(500)]
        public string? BrideFacebook { get; set; }

        [MaxLength(500)]
        public string? BrideInstagram { get; set; }

        [MaxLength(500)]
        public string? BrideX { get; set; }
        #endregion

        [MaxLength(100)]
        public string? MainSliderImageId { get; set; }

        [MaxLength(500)]
        public string? MainSliderImageUrl { get; set; }

        public DateTime EventDate { get; set; }

        public TimeSpan EventTimeFrom { get; set; }

        public TimeSpan EventTimeTo { get; set; }

        #region EventPlace Data
        [Required, MaxLength(200)]
        public string EventPlaceName { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? EventPlaceImageId { get; set; }

        [MaxLength(500)]
        public string? EventPlaceImageUrl { get; set; }

        [Required, MaxLength(500)]
        public string EventAddress { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string? LocationUrl { get; set; }
        #endregion
    }
}
