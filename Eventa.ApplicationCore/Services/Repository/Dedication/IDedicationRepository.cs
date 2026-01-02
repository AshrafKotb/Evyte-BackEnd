using Eventa.Domain.Entities;

namespace Eventa.ApplicationCore.Services.Repository
{
    public interface IDedicationRepository
    {
        Task AddDedicationAsync(Dedication dedication);
        Task AddDedicationsAsync(IEnumerable<Dedication> dedications);
        Task<IEnumerable<Dedication>> GetDedicationsByRequestIdAsync(Guid requestId);
    }
}
