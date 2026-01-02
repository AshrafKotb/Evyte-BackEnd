using Eventa.Domain.Entities;

namespace Eventa.ApplicationCore.Services.Repository
{
    public interface IRequestAudioRepository
    {
        Task AddAudioAsync(RequestAudio audio);
        Task<RequestAudio?> GetAudioByRequestIdAsync(Guid requestId);
    }
}
