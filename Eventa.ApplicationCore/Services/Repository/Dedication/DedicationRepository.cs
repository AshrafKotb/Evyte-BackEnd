using Eventa.Domain.Entities;
using Eventa.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Eventa.ApplicationCore.Services.Repository
{
    public class DedicationRepository : IDedicationRepository
    {
        private readonly ApplicationDbContext _context;

        public DedicationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddDedicationAsync(Dedication dedication)
        {
            _context.Dedications.Add(dedication);
            await _context.SaveChangesAsync();
        }

        public async Task AddDedicationsAsync(IEnumerable<Dedication> dedications)
        {
            _context.Dedications.AddRange(dedications);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Dedication>> GetDedicationsByRequestIdAsync(Guid requestId)
        {
            return await _context.Dedications
                .Where(d => d.RequestId == requestId)
                .OrderBy(d => d.DisplayOrder)
                .ToListAsync();
        }
    }
}
