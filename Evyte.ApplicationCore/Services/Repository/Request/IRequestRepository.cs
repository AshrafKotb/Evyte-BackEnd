using Evyte.ApplicationCore.Models.Helper;
using Evyte.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Evyte.ApplicationCore.Services.Repository
{
    public interface IRequestRepository
    {
        Task AddRequestAsync(Request request);
        Task<Request> GetRequestByIdAsync(Guid id);
        Task<IEnumerable<Request>> GetAllRequestsAsync();
        Task UpdateRequestAsync(Request request);
        Task DeleteRequestAsync(Guid id);
        Task<PaginatedResult<Request>> GetRequestsPaginatedAsync(int pageNumber, int pageSize, string searchTerm = "");
        Task<PaginatedResult<Request>> GetRequestsByUserIdAsync(string userId);

        Task<Request> GetRequestBySlugAsync(string slug);
    }
}
