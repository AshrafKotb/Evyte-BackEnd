using Eventa.Domain.Entities;

namespace Eventa.ApplicationCore.Services.Repository
{
    public interface IBackgroundImageRepository
    {
        Task<IEnumerable<BackgroundImage>> GetAllAsync(string? category = null);
        Task<BackgroundImage?> GetByIdAsync(Guid id);
        Task<BackgroundImage> AddAsync(BackgroundImage image);
        Task UpdateAsync(BackgroundImage image);
        Task DeleteAsync(Guid id);
    }
}
