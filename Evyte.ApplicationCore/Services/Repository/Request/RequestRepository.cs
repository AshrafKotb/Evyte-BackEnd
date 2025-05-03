using Evyte.ApplicationCore.Models.Helper;
using Evyte.Domain.Entities;
using Evyte.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Evyte.ApplicationCore.Services.Repository
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
            return await _context.Requests
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
            var request = await _context.Requests.FirstOrDefaultAsync(r => r.Id == id);
            if (request != null)
            {
                request.IsDeleted = true;
                _context.Requests.Update(request);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<PaginatedResult<Request>> GetRequestsPaginatedAsync(int pageNumber, int pageSize, string searchTerm = "")
        {
            var query = _context.Requests
                .Where(r => !r.IsDeleted);

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(r => r.UserId.ToString().Contains(searchTerm) || r.QrCodeImageUrl.Contains(searchTerm));
            }

            var totalCount = await query.CountAsync();

            var requests = await query
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
            var query = _context.Requests
                .Where(r => !r.IsDeleted && r.UserId == userId);

       
            var totalCount = await query.CountAsync();


            var requests = await query.ToListAsync();

            
            return new PaginatedResult<Request>
            {
                Items = requests,
                TotalCount = totalCount,
            };
        }
    }
}