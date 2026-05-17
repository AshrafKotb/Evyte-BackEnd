using Eventa.Domain.Entities;
using Eventa.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Eventa.ApplicationCore.Services.Repository
{
    public class SplashTemplateRepository : ISplashTemplateRepository
    {
        private readonly ApplicationDbContext _context;

        public SplashTemplateRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SplashTemplate>> GetAllAsync(bool activeOnly = false)
        {
            var query = _context.SplashTemplates.Where(s => !s.IsDeleted);

            if (activeOnly)
            {
                query = query.Where(s => s.IsActive);
            }

            return await query
                .OrderByDescending(s => s.IsDefault)
                .ThenBy(s => s.SortingNumber)
                .ThenBy(s => s.NameAr)
                .ToListAsync();
        }

        public async Task<SplashTemplate?> GetByIdAsync(Guid id)
        {
            return await _context.SplashTemplates
                .FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted);
        }

        public async Task<SplashTemplate?> GetDefaultAsync()
        {
            var def = await _context.SplashTemplates
                .FirstOrDefaultAsync(s => !s.IsDeleted && s.IsActive && s.IsDefault);

            if (def != null) return def;

            return await _context.SplashTemplates
                .Where(s => !s.IsDeleted && s.IsActive)
                .OrderBy(s => s.SortingNumber)
                .FirstOrDefaultAsync();
        }

        public async Task<SplashTemplate> AddAsync(SplashTemplate splash)
        {
            if (splash.IsDefault)
            {
                await UnsetAllDefaults();
            }

            _context.SplashTemplates.Add(splash);
            await _context.SaveChangesAsync();
            return splash;
        }

        public async Task UpdateAsync(SplashTemplate splash)
        {
            if (splash.IsDefault)
            {
                await UnsetAllDefaults(exceptId: splash.Id);
            }

            splash.UpdatedDate = DateTime.UtcNow;
            _context.SplashTemplates.Update(splash);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var splash = await _context.SplashTemplates.FindAsync(id);
            if (splash != null)
            {
                splash.IsDeleted = true;
                splash.DeletedDate = DateTime.UtcNow;
                // لو السبلاش المحذوف كان الافتراضي - نلغي علامة الافتراضي عشان مفيش حد ياخد مكانه
                splash.IsDefault = false;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ToggleActiveAsync(Guid id)
        {
            var splash = await _context.SplashTemplates
                .FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted);
            if (splash == null) return false;

            // مينفعش نوقف السبلاش الافتراضي - لازم الأدمن يحدد افتراضي تاني الأول
            if (splash.IsActive && splash.IsDefault)
            {
                return splash.IsActive;
            }

            splash.IsActive = !splash.IsActive;
            splash.UpdatedDate = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return splash.IsActive;
        }

        public async Task SetAsDefaultAsync(Guid id)
        {
            var splash = await _context.SplashTemplates
                .FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted && s.IsActive);
            if (splash == null) return;

            await UnsetAllDefaults(exceptId: id);

            splash.IsDefault = true;
            splash.UpdatedDate = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }

        private async Task UnsetAllDefaults(Guid? exceptId = null)
        {
            var currentDefaults = await _context.SplashTemplates
                .Where(s => s.IsDefault && !s.IsDeleted && (exceptId == null || s.Id != exceptId))
                .ToListAsync();

            foreach (var d in currentDefaults)
            {
                d.IsDefault = false;
            }
        }
    }
}
