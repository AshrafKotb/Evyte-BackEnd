using Evyte.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Evyte.ApplicationCore.Services.Repository
{
    public interface IRequestGalleryPhotoRepository
    {
        Task AddGalleryPhotoAsync(RequestGalleryPhoto photo);
    }
}
