using Evyte.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace Evyte.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public bool IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }
        public string FullName { get; set; }
        public DateTime JoinDate { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public UserType UserType { get; set; } = UserType.User;
        //public string ProfileImageId { get; set; }
        //public string ProfileImageUrl { get; set; }
        public IEnumerable<Request> Requests { get; set; }
    }
}