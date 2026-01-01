using Eventa.ApplicationCore.Models.ViewModels;
using Eventa.Domain.Entities;
using Eventa.Infrastructure;
using Eventa.ApplicationCore.Models.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Eventa.ApplicationCore.Services.Authantication;

namespace Eventa.Controllers
{
    [Authorize(Roles = RoleName.Admin)]
    public class cpController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly IAuthanticationService _authanticationService;

        public cpController(ApplicationDbContext context, UserManager<ApplicationUser> userManager
            , SignInManager<ApplicationUser> signInManager, IAuthanticationService authanticationService)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _authanticationService = authanticationService;
        }
        public IActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login");
            return View();
        }
        [AllowAnonymous]
        public async Task<IActionResult> Login()
        {
            await _authanticationService.CheckIfMainRolesAndUserCreatedOrNotAsync();
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _authanticationService.LoginAsync(model);

            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "?????? ?? ???? ?????? ??? ????!");
                return View(model);
            }

            if (!await _authanticationService.IsUserAdmin(model.Email))
            {
                await _authanticationService.LogoutAsync();
                ModelState.AddModelError(string.Empty, "??? ???? ?????? ??????");
                return View(model);
            }

            return RedirectToAction("Index", "cp");
        }

        public async Task<IActionResult> LogOut()
        {
            await _authanticationService.LogoutAsync();
            return RedirectToAction(nameof(Login));
        }


        public IActionResult ChangePassword()
        {
            var id = _userManager.GetUserId(User);

            if (!_context.Users.Any(u => u.Id == id))
                return View("Message", "?? ????? ?????? ??? ???????? ???? ????? ????? ???");

            ChangePasswordViewModel viewModel = new()
            {
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePasswordAsync(ChangePasswordViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);
            var id = _userManager.GetUserId(User);

            var userInDb = _context.Users.Find(id);
            if (userInDb is null)
                return View("Message", "?? ????? ?????? ??? ???????? ???? ????? ????? ???");

            var removePasswordResult = await _userManager.RemovePasswordAsync(userInDb);
            if (removePasswordResult.Succeeded)
            {
                var addNewPasswordResult = await _userManager.AddPasswordAsync(userInDb, vm.NewPassword);
                if (!addNewPasswordResult.Succeeded)
                {
                    foreach (var e in addNewPasswordResult.Errors)
                        ModelState.AddModelError(string.Empty, e.Description);
                    return View(vm);
                }
            }
            return RedirectToAction("Index", "cp");

        }



    }

}
