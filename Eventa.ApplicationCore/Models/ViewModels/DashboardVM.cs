using Eventa.Domain.Entities;

namespace Eventa.ApplicationCore.Models.ViewModels
{
    public class DashboardVM
    {
        public int TotalRequests { get; set; }
        public int TotalCustomers { get; set; }
        public int TotalDesigns { get; set; }
        public int TotalCategories { get; set; }

        public int PendingRequests { get; set; }
        public int ApprovedRequests { get; set; }
        public int RejectedRequests { get; set; }

        public List<RecentRequestVM> RecentRequests { get; set; } = new();
        public List<CategoryStatsVM> CategoryStats { get; set; } = new();
        public List<MonthlyRequestVM> MonthlyRequests { get; set; } = new();
    }

    public class RecentRequestVM
    {
        public Guid Id { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string DesignName { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public string TimeAgo { get; set; } = string.Empty;
    }

    public class CategoryStatsVM
    {
        public string CategoryName { get; set; } = string.Empty;
        public int RequestCount { get; set; }
        public string Color { get; set; } = "#6366f1";
    }

    public class MonthlyRequestVM
    {
        public string MonthName { get; set; } = string.Empty;
        public int Count { get; set; }
    }
}
