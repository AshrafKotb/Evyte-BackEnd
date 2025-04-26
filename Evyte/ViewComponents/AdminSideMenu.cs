// ViewComponents/AdminSideMenuViewComponent.cs
using Microsoft.AspNetCore.Mvc;
using Evyte.ApplicationCore.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Evyte.Domain.Entities;

public class AdminSideMenuViewComponent : ViewComponent
{
    private readonly UserManager<ApplicationUser> _userManager;

    public AdminSideMenuViewComponent(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var id = _userManager.GetUserId(HttpContext.User);
        var account = await _userManager.FindByIdAsync(id);

        var sideMenuVM = new SideMenuVM()
        {
            Name = account?.FullName,
            Email = account?.Email,
            ProfileImage =  "/images/default-profile.png"
        };

        return View(sideMenuVM);
    }
}