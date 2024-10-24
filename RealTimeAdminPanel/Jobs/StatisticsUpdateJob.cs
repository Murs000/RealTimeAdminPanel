using Quartz;
using RealTimeAdminPanel.Services;
using RealTimeAdminPanel.Models;
using System;

namespace RealTimeAdminPanel.Jobs
{
    public class StatisticsUpdateJob : IJob
    {
        private readonly StatisticsService _statisticsService;

        public StatisticsUpdateJob(StatisticsService statisticsService)
        {
            _statisticsService = statisticsService;
        }

        public Task Execute(IJobExecutionContext context)
        {
            // Get current statistics
            var currentStats = _statisticsService.GetCurrentStatistics();
            
            // Create new statistics based on logical increases
            var updatedStats = new StatisticsData
            {
                TotalUsers = currentStats.TotalUsers + GenerateNewUsers(), // Always increasing
                ActiveUsers = GenerateActiveUsers(currentStats), // Fluctuates up or down
                NewUsersToday = currentStats.NewUsersToday + GenerateNewUsersToday(), // Increases steadily
                TotalSales = currentStats.TotalSales + GenerateSalesGrowth(), // Sales growing faster
                ErrorsReported = GenerateErrorCount(currentStats) // Slow increase in errors
            };

            // Update the statistics and notify clients
            _statisticsService.UpdateStatistics(updatedStats);

            return Task.CompletedTask;
        }

        // Generate a random number of new users, simulating daily signups
        private int GenerateNewUsers()
        {
            return new Random().Next(1, 10); // Add 1-10 new users
        }

        // Simulate fluctuation in active users, can increase or decrease
        private int GenerateActiveUsers(StatisticsData currentStats)
        {
            int change = new Random().Next(-50, 50); // Active users can go up or down by 50
            int newActiveUsers = currentStats.ActiveUsers + change;
            return Math.Clamp(newActiveUsers, 0, currentStats.TotalUsers); // Ensure active users is not negative or exceeding total users
        }

        // Generate a random number of new users today
        private int GenerateNewUsersToday()
        {
            return new Random().Next(1, 5); // Add 1-5 new users today
        }

        // Simulate sales growth, increasing at a faster rate than other stats
        private decimal GenerateSalesGrowth()
        {
            return new Random().Next(100, 1000); // Sales increase by $100-$1000
        }

        // Generate error count, slower and more random than other stats
        private int GenerateErrorCount(StatisticsData currentStats)
        {
            int errorIncrease = new Random().Next(0, 2); // Errors increase by 0-2
            return currentStats.ErrorsReported + errorIncrease;
        }
    }
}