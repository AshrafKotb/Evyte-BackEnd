using Eventa.Domain.Entities;
using Eventa.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Eventa.ApplicationCore.Services.Repository
{
    public class BackgroundImageRepository : IBackgroundImageRepository
    {
        private readonly ApplicationDbContext _context;

        public BackgroundImageRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BackgroundImage>> GetAllAsync(string? category = null)
        {
            var query = _context.BackgroundImages.Where(b => !b.IsDeleted);

            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(b => b.Category == category);
            }

            return await query.OrderBy(b => b.SortingNumber).ToListAsync();
        }

        public async Task<BackgroundImage?> GetByIdAsync(Guid id)
        {
            return await _context.BackgroundImages
                .FirstOrDefaultAsync(b => b.Id == id && !b.IsDeleted);
        }

        public async Task<BackgroundImage> AddAsync(BackgroundImage image)
        {
            _context.BackgroundImages.Add(image);
            await _context.SaveChangesAsync();
            return image;
        }

        public async Task UpdateAsync(BackgroundImage image)
        {
            image.UpdatedDate = DateTime.UtcNow;
            _context.BackgroundImages.Update(image);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var image = await _context.BackgroundImages.FindAsync(id);
            if (image != null)
            {
                image.IsDeleted = true;
                image.DeletedDate = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }
    }
}
