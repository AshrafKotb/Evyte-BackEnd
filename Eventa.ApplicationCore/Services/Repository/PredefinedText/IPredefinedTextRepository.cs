using Eventa.Domain.Entities;

namespace Eventa.ApplicationCore.Services.Repository
{
    public interface IPredefinedTextRepository
    {
        Task<IEnumerable<PredefinedText>> GetAllAsync(string? textType = null);
        Task<PredefinedText?> GetByIdAsync(Guid id);
        Task<PredefinedText> AddAsync(PredefinedText text);
        Task UpdateAsync(PredefinedText text);
        Task DeleteAsync(Guid id);
    }
}
