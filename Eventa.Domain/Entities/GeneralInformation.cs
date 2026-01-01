using System.ComponentModel.DataAnnotations;

namespace Eventa.Domain.Entities
{
    public class GeneralInformation : BaseEntity
    {
        #region Social Media Data
        [MaxLength(500)]
        public string? AddressAr { get; set; }

        [MaxLength(500)]
        public string? AddressEn { get; set; }

        [MaxLength(500)]
        public string? FaceBook { get; set; }

        [MaxLength(500)]
        public string? Instagram { get; set; }

        [MaxLength(500)]
        public string? X { get; set; }

        [MaxLength(500)]
        public string? Tiktok { get; set; }

        [MaxLength(500)]
        public string? Youtube { get; set; }

        [MaxLength(50)]
        public string? WhatsApp { get; set; }

        [MaxLength(500)]
        public string? Snapchat { get; set; }

        [EmailAddress, MaxLength(200)]
        public string? Email { get; set; }

        [EmailAddress, MaxLength(200)]
        public string? Email2 { get; set; }

        [Phone, MaxLength(20)]
        public string? PhoneNumber { get; set; }

        [Phone, MaxLength(20)]
        public string? PhoneNumber2 { get; set; }
        #endregion

        [MaxLength(10000)]
        public string? TermsAndConditionsAr { get; set; }

        [MaxLength(10000)]
        public string? TermsAndConditionsEn { get; set; }
    }
}
