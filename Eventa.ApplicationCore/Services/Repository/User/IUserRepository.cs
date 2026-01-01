using Eventa.ApplicationCore.Models.Helper;
using Eventa.ApplicationCore.Models.ViewModels;
using Eventa.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Eventa.ApplicationCore.Services.Repository
{
    public interface IUserRepository
    {
        Task<ApplicationUser> GetUserByEmailAndPhoneAsync(string email, string phone);
        Task<string> CreateUserAsync(ApplicationUser user);
        Task<PaginatedResult<CustomerViewModel>> GetAllCustomersWithRequestsAsync(int pageNumber, int pageSize, string searchTerm = "");
    }
}
