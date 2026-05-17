using System.ComponentModel.DataAnnotations;

namespace Eventa.Domain.Entities
{
    public class ClientInfo : BaseEntity
    {
        [Required, MaxLength(200)]
        public string FullName { get; set; } = string.Empty;

        [Required, MaxLength(20)]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required, MaxLength(20)]
        public string WhatsAppNumber { get; set; } = string.Empty;

        [Required, MaxLength(200)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public virtual ICollection<Request> Requests { get; set; } = new List<Request>();
    }
}
