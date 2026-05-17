using Eventa.Domain.Entities;
using Eventa.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Eventa.ApplicationCore.Services.Repository
{
    public class PredefinedTextRepository : IPredefinedTextRepository
    {
        private readonly ApplicationDbContext _context;

        public PredefinedTextRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PredefinedText>> GetAllAsync(string? textType = null)
        {
            var query = _context.PredefinedTexts.Where(p => !p.IsDeleted);

            if (!string.IsNullOrEmpty(textType))
            {
                query = query.Where(p => p.TextType == textType);
            }

            return await query.OrderBy(p => p.SortingNumber).ToListAsync();
        }

        public async Task<PredefinedText?> GetByIdAsync(Guid id)
        {
            return await _context.PredefinedTexts
                .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);
        }

        public async Task<PredefinedText> AddAsync(PredefinedText text)
        {
            _context.PredefinedTexts.Add(text);
            await _context.SaveChangesAsync();
            return text;
        }

        public async Task UpdateAsync(PredefinedText text)
        {
            text.UpdatedDate = DateTime.UtcNow;
            _context.PredefinedTexts.Update(text);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var text = await _context.PredefinedTexts.FindAsync(id);
            if (text != null)
            {
                text.IsDeleted = true;
                text.DeletedDate = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }
    }
}
