using Eventa.ApplicationCore.Models.Helper;
using Eventa.Domain.Entities;
using Eventa.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace Eventa.ApplicationCore.Services.Repository
{
    public interface IRequestRepository
    {
        Task AddRequestAsync(Request request);
        Task<Request> GetRequestByIdAsync(Guid id);
        Task<IEnumerable<Request>> GetAllRequestsAsync();
        Task UpdateRequestAsync(Request request);
        Task DeleteRequestAsync(Guid id);
        Task<PaginatedResult<Request>> GetRequestsPaginatedAsync(int pageNumber, int pageSize, string searchTerm = "", InvitationStatus? status = null);
        Task<PaginatedResult<Request>> GetRequestsByUserIdAsync(string userId);

        Task<Request> GetRequestBySlugAsync(string slug);
        Task<PaginatedResult<Request>> GetDeletedRequestsPaginatedAsync(int pageNumber, int pageSize, string searchTerm = "");
        Task RestoreRequestAsync(Guid id);
        Task PermanentDeleteRequestAsync(Guid id);
    }
}
