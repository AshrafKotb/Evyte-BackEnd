using Eventa.Domain.Entities;
using Eventa.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Eventa.ApplicationCore.Services.Repository
{
    public class MemoryRepository : IMemoryRepository
    {
        private readonly ApplicationDbContext _context;

        public MemoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddMemoryAsync(Memory memory)
        {
            _context.Memories.Add(memory);
            await _context.SaveChangesAsync();
        }

        public async Task AddMemoriesAsync(IEnumerable<Memory> memories)
        {
            _context.Memories.AddRange(memories);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Memory>> GetMemoriesByRequestIdAsync(Guid requestId)
        {
            return await _context.Memories
                .Where(m => m.RequestId == requestId)
                .OrderBy(m => m.DisplayOrder)
                .ToListAsync();
        }
    }
}
