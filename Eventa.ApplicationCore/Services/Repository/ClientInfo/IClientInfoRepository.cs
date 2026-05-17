using Eventa.Domain.Entities;

namespace Eventa.ApplicationCore.Services.Repository
{
    public interface IClientInfoRepository
    {
        Task<ClientInfo> AddClientInfoAsync(ClientInfo clientInfo);
        Task<ClientInfo?> GetClientInfoByIdAsync(Guid id);
        Task<ClientInfo?> GetClientInfoByEmailAsync(string email);
    }
}
