using Evyte.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Evyte.ApplicationCore.Services.Repository
{
    public interface IRequestRepository
    {
        Task AddRequestAsync(Request request);
    }
}
