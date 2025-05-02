using Evyte.ApplicationCore.Services.Files;
using Evyte.ApplicationCore.Services.Repository;
using Evyte.Domain.Entities;
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
}