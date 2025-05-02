using Evyte.Domain.Entities;
using Evyte.Infrastructure;
using Microsoft.AspNetCore.Http;

namespace Evyte.ApplicationCore.Services.Repository
{
    public class RequestRepository : IRequestRepository
    {
        private readonly ApplicationDbContext _context;

        public RequestRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Add a new request
        public async Task AddRequestAsync(Request request)
        {
            _context.Requests.Add(request);
            var res = await _context.SaveChangesAsync();
        }
    }
}
