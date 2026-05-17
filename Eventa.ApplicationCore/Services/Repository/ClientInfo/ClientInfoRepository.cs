using Eventa.Domain.Entities;
using Eventa.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Eventa.ApplicationCore.Services.Repository
{
    public class ClientInfoRepository : IClientInfoRepository
    {
        private readonly ApplicationDbContext _context;

        public ClientInfoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ClientInfo> AddClientInfoAsync(ClientInfo clientInfo)
        {
            _context.ClientInfos.Add(clientInfo);
            await _context.SaveChangesAsync();
            return clientInfo;
        }

        public async Task<ClientInfo?> GetClientInfoByIdAsync(Guid id)
        {
            return await _context.ClientInfos
                .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);
        }

        public async Task<ClientInfo?> GetClientInfoByEmailAsync(string email)
        {
            return await _context.ClientInfos
                .FirstOrDefaultAsync(c => c.Email == email && !c.IsDeleted);
        }
    }
}
