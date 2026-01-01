using Eventa.Domain.Entities;
using Eventa.Infrastructure;
using Microsoft.AspNetCore.Http;

namespace Eventa.ApplicationCore.Services.Repository
{
    public class RequestGalleryPhotoRepository : IRequestGalleryPhotoRepository
    {
        private readonly ApplicationDbContext _context;

        public RequestGalleryPhotoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Add a new gallery photo
        public async Task AddGalleryPhotoAsync(RequestGalleryPhoto photo)
        {
            _context.RequestsGallery.Add(photo);
            await _context.SaveChangesAsync();
        }
    }
}
