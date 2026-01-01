using Eventa.Domain.Entities;
using Eventa.Infrastructure;
using Microsoft.AspNetCore.Http;

namespace Eventa.ApplicationCore.Services.Repository
{
    public class RequestDataRepository : IRequestDataRepository
    {
        private readonly ApplicationDbContext _context;

        public RequestDataRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Add new request data
        public async Task AddRequestDataAsync(RequestData requestData)
        {
            _context.RequestsData.Add(requestData);
            await _context.SaveChangesAsync();
        }
    }
}
