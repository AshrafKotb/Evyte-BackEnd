using Eventa.Domain.Entities;

namespace Eventa.ApplicationCore.Services.Repository
{
    public interface ISplashTemplateRepository
    {
        // كل السبلاشات (للأدمن). activeOnly = true يرجع المُفعّلة فقط (للعملاء)
        Task<IEnumerable<SplashTemplate>> GetAllAsync(bool activeOnly = false);

        Task<SplashTemplate?> GetByIdAsync(Guid id);

        // السبلاش الافتراضي العام (IsDefault=true وIsActive=true). لو مفيش يرجع أول واحد مُفعّل
        Task<SplashTemplate?> GetDefaultAsync();

        Task<SplashTemplate> AddAsync(SplashTemplate splash);

        Task UpdateAsync(SplashTemplate splash);

        Task DeleteAsync(Guid id);

        // يبدّل IsActive للسبلاش - يرجع الحالة الجديدة. لو هيتم تعطيل السبلاش الافتراضي بنرفض
        Task<bool> ToggleActiveAsync(Guid id);

        // يعمل هذا السبلاش هو الافتراضي ويلغي الافتراضي عن أي حد تاني (لازم يكون Active)
        Task SetAsDefaultAsync(Guid id);
    }
}
