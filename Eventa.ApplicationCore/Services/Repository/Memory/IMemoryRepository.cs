using Eventa.Domain.Entities;

namespace Eventa.ApplicationCore.Services.Repository
{
    public interface IMemoryRepository
    {
        Task AddMemoryAsync(Memory memory);
        Task AddMemoriesAsync(IEnumerable<Memory> memories);
        Task<IEnumerable<Memory>> GetMemoriesByRequestIdAsync(Guid requestId);
    }
}
