using System.Threading.Tasks;
using Evyte.ApplicationCore.Models.ViewModels;
using Evyte.Domain.Entities;
using Evyte.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Evyte.Web.ViewComponents
{
    public class HeaderViewComponent : ViewComponent
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<HeaderViewComponent> _logger;

        public HeaderViewComponent(
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context,
            ILogger<HeaderViewComponent> logger)
        {
            _userManager = userManager;
            _context = context;
            _logger = logger;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            try
            {
                var userId = _userManager.GetUserId(HttpContext.User);
                var model = new HeaderVM();

                if (!string.IsNullOrEmpty(userId))
                {
                    var account = await _userManager.FindByIdAsync(userId);
                    if (account != null)
                    {
                        model.Name = account.FullName;

                        // جلب الإشعارات
                        var notifications = await _context.Notifications
                            .Where(n => !n.IsDeleted)
                            .OrderByDescending(n => n.CreatedDate)
                            .Take(10)
                            .ToListAsync();

                        model.NotificationsNumber = notifications.Count(n => !n.IsSeen);
                        model.Notifications = notifications.Select(n => new NotificationVM
                        {
                            Id = n.Id,
                            Title = n.Title,
                            Body = n.Body,
                            Link = n.NotificationLink,
                            IsSeen = n.IsSeen,
                            CreatedDate = n.CreatedDate.ToString("g")
                        }).ToList();
                    }
                }

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in HeaderViewComponent");
                return View(new HeaderVM());
            }
        }
    }
}