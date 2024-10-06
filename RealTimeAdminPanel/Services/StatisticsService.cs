using System;
using Bogus;

namespace RealTimeAdminPanel.Services
{
    public class StatisticsService
    {
        private StatisticsData _currentStatistics;

        // Initialize with random mock data using Bogus
        public StatisticsService()
        {
            _currentStatistics = GenerateRandomStatistics();
        }

        // Method to get current statistics data
        public StatisticsData GetCurrentStatistics()
        {
            return _currentStatistics;
        }

        // Method to update statistics data
        public void UpdateStatistics(StatisticsData newData)
        {
            _currentStatistics = newData;
        }

        // Method to generate random statistics data
        public StatisticsData GenerateRandomStatistics()
        {
            var faker = new Faker<StatisticsData>()
                .RuleFor(s => s.TotalUsers, f => f.Random.Int(1000, 5000))
                .RuleFor(s => s.ActiveUsers, f => f.Random.Int(100, 1000))
                .RuleFor(s => s.NewUsersToday, f => f.Random.Int(10, 100))
                .RuleFor(s => s.TotalSales, f => f.Finance.Amount(10000, 50000))
                .RuleFor(s => s.ErrorsReported, f => f.Random.Int(0, 10));

            return faker.Generate();
        }
    }
}