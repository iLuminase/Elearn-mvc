using System.Collections.Generic;

namespace EbookMVC.Models
{
    public class DashboardViewModel
    {
        public int TotalProducts { get; set; }
        public int TotalCategories { get; set; }
        public decimal TotalRevenue { get; set; }
        public int TotalUsers { get; set; }
        public List<UserLog> RecentActivities { get; set; }
        public Dictionary<string, int> ProductsByCategory { get; set; }
        public Dictionary<string, int> ActionCounts { get; set; }
    }
}
