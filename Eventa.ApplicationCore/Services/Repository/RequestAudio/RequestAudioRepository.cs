using Eventa.Domain.Entities;
using Eventa.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Eventa.ApplicationCore.Services.Repository
{
    public class RequestAudioRepository : IRequestAudioRepository
    {
        private readonly ApplicationDbContext _context;

        public RequestAudioRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAudioAsync(RequestAudio audio)
        {
            _context.RequestAudios.Add(audio);
            await _context.SaveChangesAsync();
        }

        public async Task<RequestAudio?> GetAudioByRequestIdAsync(Guid requestId)
        {
            return await _context.RequestAudios
                .FirstOrDefaultAsync(a => a.RequestId == requestId);
        }
    }
}
