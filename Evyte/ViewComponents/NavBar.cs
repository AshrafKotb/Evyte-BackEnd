using System.Threading.Tasks;
using Evyte.ApplicationCore.Models.ViewModels;
using Evyte.Domain.Entities;
using Evyte.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Evyte.Web.ViewComponents
{
    public class NavBarViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}