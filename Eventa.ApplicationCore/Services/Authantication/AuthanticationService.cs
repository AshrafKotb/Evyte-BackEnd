using Eventa.ApplicationCore.Services.Files;
using Eventa.ApplicationCore.Services.Mailing;
using Eventa.Domain.Entities;
using Eventa.ApplicationCore.Models.Helper;
using Eventa.Infrastructure;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eventa.ApplicationCore.Models.ViewModels;
using Eventa.Domain.Enums;

namespace Eventa.ApplicationCore.Services.Authantication
{
    public class AuthanticationService : IAuthanticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;

        public AuthanticationService(ApplicationDbContext context, UserManager<ApplicationUser> userManager
            , RoleManager<IdentityRole> roleManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _signInManager = signInManager;
        }

        public ApplicationUser FindByEmail(string Email)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == Email);
            return user;
        }
        public async Task CheckIfMainRolesAndUserCreatedOrNotAsync()
        {
            List<string> roles = new() { RoleName.Admin, RoleName.User };

            foreach (var roleName in roles)
            {
                if (!await _roleManager.RoleExistsAsync(roleName))
                {
                    await _roleManager.CreateAsync(new IdentityRole { Name = roleName });
                }
            }

            var isAdminExist = FindByEmail("Admin@Eventa.com");
            if (isAdminExist is null)
            {
                ApplicationUser adminUser = new()
                {
                    Email = "Admin@Eventa.com",
                    UserName = "Admin@Eventa.com",
                    FullName = "Admin",
                    UserType = UserType.Admin,
                    PhoneNumber = "0123456789",
                    JoinDate = DateTime.UtcNow,
                };

                var res = await _userManager.CreateAsync(adminUser, "123456");
                if (res.Succeeded)
                    await _userManager.AddToRoleAsync(adminUser, RoleName.Admin);
            }

            var isAdmin2Exist = FindByEmail("Dev@Eventa.com");
            if (isAdmin2Exist is null)
            {
                ApplicationUser admin2User = new()
                {
                    Email = "Dev@Eventa.com",
                    UserName = "Dev@Eventa.com",
                    FullName = "Admin2",
                    PhoneNumber = "01234567890",
                    UserType = UserType.Admin,
                    JoinDate = DateTime.UtcNow,
                };

                var res2 = await _userManager.CreateAsync(admin2User, "123456");
                if (res2.Succeeded)
                    await _userManager.AddToRoleAsync(admin2User, RoleName.Admin);
            }
        }

        public async Task<SignInResult> LoginAsync(LoginViewModel model)
        {
            var user = FindByEmail(model.Email);

            if (user == null || user.UserType != UserType.Admin)
            {
                return SignInResult.Failed;
            }

            return await _signInManager.PasswordSignInAsync(
                user,
                model.Password,
                model.RememberMe,
                lockoutOnFailure: false);
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<bool> IsUserAdmin(string email)
        {
            var user = FindByEmail(email);
            if (user == null || user.UserType != UserType.Admin) return false;

            return await _userManager.IsInRoleAsync(user, RoleName.Admin);
        }
    }
}
