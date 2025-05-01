using Evyte.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Evyte.ApplicationCore.Services.Repository
{
    public interface IRequestDataRepository
    {
        Task AddRequestDataAsync(RequestData requestData);
    }
}
