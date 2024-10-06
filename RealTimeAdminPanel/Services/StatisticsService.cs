using Microsoft.AspNetCore.SignalR;
using RealTimeAdminPanel.Models;
using RealTimeAdminPanel.Hubs;

namespace RealTimeAdminPanel.Services
{
    public class StatisticsService
    {
        private StatisticsData _currentStatistics;
        private readonly IHubContext<StatisticsHub> _hubContext;

        public StatisticsService(IHubContext<StatisticsHub> hubContext)
        {
            _hubContext = hubContext;
            _currentStatistics = GenerateRandomStatistics();
        }

        // Get current statistics
        public StatisticsData GetCurrentStatistics()
        {
            return _currentStatistics;
        }

        // Update statistics data and notify all connected clients
        public void UpdateStatistics(StatisticsData newData)
        {
            _currentStatistics = newData;
            _hubContext.Clients.All.SendAsync("ReceiveStatistics", _currentStatistics);
        }

        // Generate random statistics
        public StatisticsData GenerateRandomStatistics()
        {
            var faker = new Bogus.Faker<StatisticsData>()
                .RuleFor(s => s.TotalUsers, f => f.Random.Int(1000, 5000))
                .RuleFor(s => s.ActiveUsers, f => f.Random.Int(100, 1000))
                .RuleFor(s => s.NewUsersToday, f => f.Random.Int(10, 100))
                .RuleFor(s => s.TotalSales, f => f.Finance.Amount(10000, 50000))
                .RuleFor(s => s.ErrorsReported, f => f.Random.Int(0, 10));

            return faker.Generate();
        }
    }
}