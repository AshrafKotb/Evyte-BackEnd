using Eventa.ApplicationCore.Models.ViewModels;
using Eventa.Domain.Entities;
using Eventa.Domain.Enums;
using Eventa.Infrastructure;
using Eventa.ApplicationCore.Models.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public async Task<IActionResult> Index()
        {
            if (!User.Identity!.IsAuthenticated)
                return RedirectToAction("Login");

            var viewModel = await GetDashboardDataAsync();
            return View(viewModel);
        }

        private async Task<DashboardVM> GetDashboardDataAsync()
        {
            var now = DateTime.Now;
            var sixMonthsAgo = now.AddMonths(-6);

            // Get counts
            var totalRequests = await _context.Requests.CountAsync(r => !r.IsDeleted);
            var totalCustomers = await _context.Users.CountAsync(u => u.Requests.Any(r => !r.IsDeleted));
            var totalDesigns = await _context.Designs.CountAsync(d => !d.IsDeleted);
            var totalCategories = await _context.Categories.CountAsync(c => !c.IsDeleted);

            // Request status counts
            var pendingRequests = await _context.Requests.CountAsync(r => !r.IsDeleted && r.Status == InvitationStatus.Pending);
            var approvedRequests = await _context.Requests.CountAsync(r => !r.IsDeleted && r.Status == InvitationStatus.Approved);
            var rejectedRequests = await _context.Requests.CountAsync(r => !r.IsDeleted && r.Status == InvitationStatus.Rejected);

            // Recent requests
            var recentRequests = await _context.Requests.Where(r => !r.IsDeleted)
                .Include(r => r.User)
                .Include(r => r.Design)
                .ThenInclude(d => d!.Category)
                .OrderByDescending(r => r.CreatedDate)
                .Take(5)
                .Select(r => new RecentRequestVM
                {
                    Id = r.Id,
                    CustomerName = r.User != null ? r.User.FullName ?? "غير معروف" : "غير معروف",
                    DesignName = r.Design != null ? r.Design.NameAr : "غير محدد",
                    CategoryName = r.Design != null && r.Design.Category != null ? r.Design.Category.NameAr : "غير محدد",
                    CreatedDate = r.CreatedDate,
                    Status = r.Status.ToString(),
                    TimeAgo = GetTimeAgo(r.CreatedDate)
                })
                .ToListAsync();

            // Category stats
            var categoryStats = await _context.Categories
                .Where(c => !c.IsDeleted)
                .Select(c => new CategoryStatsVM
                {
                    CategoryName = c.NameAr,
                    RequestCount = c.Designs.SelectMany(d => d.Requests).Count(r => !r.IsDeleted)
                })
                .OrderByDescending(c => c.RequestCount)
                .Take(5)
                .ToListAsync();

            // Assign colors to categories
            var colors = new[] { "#6366f1", "#d4a574", "#10b981", "#f43f5e", "#f59e0b" };
            for (int i = 0; i < categoryStats.Count; i++)
            {
                categoryStats[i].Color = colors[i % colors.Length];
            }

            // Monthly requests for the last 6 months
            var monthlyRequests = new List<MonthlyRequestVM>();
            var arabicMonths = new[] { "يناير", "فبراير", "مارس", "أبريل", "مايو", "يونيو", "يوليو", "أغسطس", "سبتمبر", "أكتوبر", "نوفمبر", "ديسمبر" };

            for (int i = 5; i >= 0; i--)
            {
                var month = now.AddMonths(-i);
                var startOfMonth = new DateTime(month.Year, month.Month, 1);
                var endOfMonth = startOfMonth.AddMonths(1);

                var count = await _context.Requests
                    .CountAsync(r => !r.IsDeleted && r.CreatedDate >= startOfMonth && r.CreatedDate < endOfMonth);

                monthlyRequests.Add(new MonthlyRequestVM
                {
                    MonthName = arabicMonths[month.Month - 1],
                    Count = count
                });
            }

            return new DashboardVM
            {
                TotalRequests = totalRequests,
                TotalCustomers = totalCustomers,
                TotalDesigns = totalDesigns,
                TotalCategories = totalCategories,
                PendingRequests = pendingRequests,
                ApprovedRequests = approvedRequests,
                RejectedRequests = rejectedRequests,
                RecentRequests = recentRequests,
                CategoryStats = categoryStats,
                MonthlyRequests = monthlyRequests
            };
        }

        private static string GetTimeAgo(DateTime dateTime)
        {
            var timeSpan = DateTime.Now - dateTime;

            if (timeSpan.TotalMinutes < 1)
                return "الآن";
            if (timeSpan.TotalMinutes < 60)
                return $"منذ {(int)timeSpan.TotalMinutes} دقيقة";
            if (timeSpan.TotalHours < 24)
                return $"منذ {(int)timeSpan.TotalHours} ساعة";
            if (timeSpan.TotalDays < 7)
                return $"منذ {(int)timeSpan.TotalDays} يوم";
            if (timeSpan.TotalDays < 30)
                return $"منذ {(int)(timeSpan.TotalDays / 7)} أسبوع";

            return dateTime.ToString("yyyy/MM/dd");
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
                ModelState.AddModelError(string.Empty, "خطأ في البريد الإلكتروني أو كلمة المرور!");
                return View(model);
            }

            if (!await _authanticationService.IsUserAdmin(model.Email))
            {
                await _authanticationService.LogoutAsync();
                ModelState.AddModelError(string.Empty, "غير مصرح لك بالدخول");
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
                return View("Message", "لم يتمكن النظام من العثور على حسابك، يرجى تسجيل الدخول مرة أخرى");

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
                return View("Message", "لم يتمكن النظام من العثور على حسابك، يرجى تسجيل الدخول مرة أخرى");

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
