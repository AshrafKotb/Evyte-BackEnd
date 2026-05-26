using Eventa.ApplicationCore.Models.Helper;
using Eventa.Domain.Entities;
using Eventa.Domain.Enums;
using Eventa.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Eventa.ApplicationCore.Services.Repository
{
    public class RequestRepository : IRequestRepository
    {
        private readonly ApplicationDbContext _context;

        public RequestRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddRequestAsync(Request request)
        {
            _context.Requests.Add(request);
            await _context.SaveChangesAsync();
        }

        public async Task<Request> GetRequestByIdAsync(Guid id)
        {
            return await _context.Requests.Include(x => x.User).Include(x => x.ClientInfo).Include(x => x.RequestData).Include(x => x.GalleryPhotos).Include(x => x.Design)
                .FirstOrDefaultAsync(r => r.Id == id && !r.IsDeleted);
        }

        public async Task<IEnumerable<Request>> GetAllRequestsAsync()
        {
            return await _context.Requests
                .Where(r => !r.IsDeleted)
                .ToListAsync();
        }

        public async Task UpdateRequestAsync(Request request)
        {
            _context.Requests.Update(request);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteRequestAsync(Guid id)
        {
            var request = await _context.Requests.Include(x => x.User).Include(x => x.RequestData).Include(x => x.GalleryPhotos).Include(x => x.Design)
                .FirstOrDefaultAsync(r => r.Id == id);
            if (request != null)
            {
                request.IsDeleted = true;
                _context.Requests.Update(request);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<PaginatedResult<Request>> GetRequestsPaginatedAsync(int pageNumber, int pageSize, string searchTerm = "", InvitationStatus? status = null)
        {
            var query = _context.Requests.Include(x => x.User).Include(x => x.ClientInfo).Include(x => x.RequestData).Include(x => x.GalleryPhotos).Include(x => x.Design)
                .Where(r => !r.IsDeleted);

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(r =>
                    (r.User != null && (r.User.FullName.Contains(searchTerm) ||
                    r.User.Email.Contains(searchTerm) ||
                    r.User.PhoneNumber.Contains(searchTerm))) ||
                    (r.ClientInfo != null && (r.ClientInfo.FullName.Contains(searchTerm) ||
                    r.ClientInfo.Email.Contains(searchTerm) ||
                    r.ClientInfo.PhoneNumber.Contains(searchTerm))) ||
                    r.RequestData.GroomName.Contains(searchTerm) ||
                    r.RequestData.BrideName.Contains(searchTerm) ||
                    r.RequestData.EventPlaceName.Contains(searchTerm));
            }

            if (status.HasValue)
            {
                query = query.Where(r => r.Status == status.Value);
            }

            var totalCount = await query.CountAsync();

            var requests = await query
                .OrderByDescending(r => r.CreatedDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedResult<Request>
            {
                Items = requests,
                TotalCount = totalCount,
                CurrentPage = pageNumber,
                PageSize = pageSize
            };
        }
        public async Task<PaginatedResult<Request>> GetRequestsByUserIdAsync(string userId)
        {
            var query = _context.Requests.Include(x => x.User).Include(x => x.RequestData).Include(x => x.GalleryPhotos).Include(x => x.Design)
                .Where(r => !r.IsDeleted && r.UserId == userId);


            var totalCount = await query.CountAsync();


            var requests = await query.ToListAsync();


            return new PaginatedResult<Request>
            {
                Items = requests,
                TotalCount = totalCount,
            };
        }
        public async Task<Request> GetRequestBySlugAsync(string slug)
        {
            return await _context.Requests
                .Include(r => r.RequestData)
                .Include(r => r.User)
                .Include(r => r.ClientInfo)
                .Include(r => r.Design)
                .Include(r => r.GalleryPhotos)
                .Where(r => !r.IsDeleted && r.WeddingSlug == slug)
                .OrderBy(r => r.Status == InvitationStatus.Approved ? 0 : 1)
                .ThenByDescending(r => r.ApprovedDate ?? r.CreatedDate)
                .FirstOrDefaultAsync();
        }

        public async Task<PaginatedResult<Request>> GetDeletedRequestsPaginatedAsync(int pageNumber, int pageSize, string searchTerm = "")
        {
            var query = _context.Requests
                .Include(x => x.User)
                .Include(x => x.ClientInfo)
                .Include(x => x.RequestData)
                .Include(x => x.Design)
                .Where(r => r.IsDeleted);

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(r =>
                    (r.User != null && (r.User.FullName.Contains(searchTerm) ||
                    r.User.Email.Contains(searchTerm) ||
                    r.User.PhoneNumber.Contains(searchTerm))) ||
                    (r.ClientInfo != null && (r.ClientInfo.FullName.Contains(searchTerm) ||
                    r.ClientInfo.Email.Contains(searchTerm) ||
                    r.ClientInfo.PhoneNumber.Contains(searchTerm))) ||
                    (r.RequestData != null && (r.RequestData.GroomName.Contains(searchTerm) ||
                    r.RequestData.BrideName.Contains(searchTerm))));
            }

            var totalCount = await query.CountAsync();

            var requests = await query
                .OrderByDescending(r => r.DeletedDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedResult<Request>
            {
                Items = requests,
                TotalCount = totalCount,
                CurrentPage = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task RestoreRequestAsync(Guid id)
        {
            var request = await _context.Requests.FirstOrDefaultAsync(r => r.Id == id && r.IsDeleted);
            if (request != null)
            {
                request.IsDeleted = false;
                request.DeletedDate = default;
                await _context.SaveChangesAsync();
            }
        }

        public async Task PermanentDeleteRequestAsync(Guid id)
        {
            var request = await _context.Requests
                .Include(r => r.RequestData)
                .Include(r => r.GalleryPhotos)
                .FirstOrDefaultAsync(r => r.Id == id && r.IsDeleted);
            if (request != null)
            {
                _context.Requests.Remove(request);
                await _context.SaveChangesAsync();
            }
        }
    }
}