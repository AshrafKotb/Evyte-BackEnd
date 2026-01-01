using Eventa.ApplicationCore.Models.ViewModels;
using Eventa.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eventa.ApplicationCore.Services.Authantication
{
    public interface IAuthanticationService
    {
        ApplicationUser FindByEmail(string Email);
        Task CheckIfMainRolesAndUserCreatedOrNotAsync();
        Task<SignInResult> LoginAsync(LoginViewModel model);
        Task LogoutAsync();
        Task<bool> IsUserAdmin(string email);
    }
}
