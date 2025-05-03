using Evyte.ApplicationCore.Models.Helper;
using Evyte.ApplicationCore.Models.ViewModels;
using Evyte.ApplicationCore.Services.Files;
using Evyte.ApplicationCore.Services.Repository;
using Evyte.Domain.Entities;
using Evyte.Domain.Enums;
using Evyte.Infrastructure;
using Microsoft.EntityFrameworkCore;
using QRCoder;
using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;

public class UserRepository : IUserRepository
{


    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApplicationUser> GetUserByEmailAndPhoneAsync(string email, string phone)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email /*&& u.PhoneNumber == phone */&& !u.IsDeleted);
    }

    public async Task<string> CreateUserAsync(ApplicationUser user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user.Id;
    }
    //public async Task<IEnumerable<CustomerViewModel>> GetAllCustomersWithRequestsAsync()
    //{
    //    var customers = await _context.Users
    //        .Where(u => !u.IsDeleted)
    //        .Select(u => new CustomerViewModel
    //        {
    //            FullName = u.FullName,
    //            PhoneNumber = u.PhoneNumber,
    //            Email = u.Email,
    //            RequestCount = u.Requests.Count(),
    //            QrCodeImageUrls = u.Requests.Select(r => r.QrCodeImageUrl)
    //        })
    //        .ToListAsync();

    //    return customers;
    //}
    public async Task<PaginatedResult<CustomerViewModel>> GetAllCustomersWithRequestsAsync(int pageNumber, int pageSize, string searchTerm = "")
    {
        var query = _context.Users
            .Include(u => u.Requests)
            .Where(u => u.UserType == UserType.User);

        if (!string.IsNullOrEmpty(searchTerm))
        {
            query = query.Where(u => u.FullName.Contains(searchTerm) || u.Email.Contains(searchTerm) || u.PhoneNumber.Contains(searchTerm));
        }

        var totalCount = await query.CountAsync();

        var customers = await query
            .Select(u => new CustomerViewModel
            {
                FullName = u.FullName,
                PhoneNumber = u.PhoneNumber,
                Email = u.Email,
                RequestCount = u.Requests.Count(r => !r.IsDeleted),
                Requests = u.Requests
                    .Where(r => !r.IsDeleted)
                    .Select(r => new RequestQrCodeViewModel
                    {
                        QrCodeImageUrl = r.QrCodeImageUrl,
                        DomainUrl = r.DomainUrl
                    })
            })
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PaginatedResult<CustomerViewModel>
        {
            Items = customers,
            TotalCount = totalCount,
            CurrentPage = pageNumber,
            PageSize = pageSize
        };
    }
}