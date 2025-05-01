using Evyte.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Evyte.ApplicationCore.Services.Repository
{
    public interface IUserRepository
    {
        Task<ApplicationUser> GetUserByEmailAndPhoneAsync(string email, string phone);
        Task<string> CreateUserAsync(ApplicationUser user);
    }
}
