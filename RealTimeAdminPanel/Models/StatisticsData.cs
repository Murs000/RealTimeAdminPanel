namespace RealTimeAdminPanel.Models
{
    public class StatisticsData
    {
        public int TotalUsers { get; set; }
        public int ActiveUsers { get; set; }
        public int NewUsersToday { get; set; }
        public decimal TotalSales { get; set; }
        public int ErrorsReported { get; set; }
    }
}