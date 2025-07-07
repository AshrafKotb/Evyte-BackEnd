using System.Threading.Tasks;
using Evyte.ApplicationCore.Interfaces.Services.General_Information;
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
        private readonly IGeneralInformationService _generalInformationService;

        public NavBarViewComponent(IGeneralInformationService generalInformationService)
        {
            _generalInformationService = generalInformationService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var generalInfo = await _generalInformationService.GetGeneralInformationAsync();

            return View(generalInfo);
        }
    }
}