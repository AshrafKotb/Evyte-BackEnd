using Evyte.ApplicationCore.Models.Helper;
using Evyte.ApplicationCore.Models.ViewModels;
using Evyte.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Evyte.ApplicationCore.Services.Repository
{
    public interface IUserRepository
    {
        Task<ApplicationUser> GetUserByEmailAndPhoneAsync(string email, string phone);
        Task<string> CreateUserAsync(ApplicationUser user);
        Task<PaginatedResult<CustomerViewModel>> GetAllCustomersWithRequestsAsync(int pageNumber, int pageSize, string searchTerm = "");
    }
}
