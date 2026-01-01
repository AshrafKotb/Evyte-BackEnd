using Eventa.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Eventa.ApplicationCore.Services.Repository
{
    public interface IRequestDataRepository
    {
        Task AddRequestDataAsync(RequestData requestData);
    }
}
