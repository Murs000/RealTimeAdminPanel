using Microsoft.AspNetCore.SignalR;
using RealTimeAdminPanel.Services;
using System.Threading.Tasks;

namespace RealTimeAdminPanel.Hubs
{
    public class StatisticsHub : Hub
    {
        private readonly StatisticsService _statisticsService;

        public StatisticsHub(StatisticsService statisticsService)
        {
            _statisticsService = statisticsService;
        }

        // Send initial data when a client connects
        public override async Task OnConnectedAsync()
        {
            // Get the current statistics data
            var currentData = _statisticsService.GetCurrentStatistics();
            // Send it to the newly connected client
            await Clients.Caller.SendAsync("ReceiveStatistics", currentData);
            
            await base.OnConnectedAsync();
        }

        // Method to push real-time statistics data to all clients
        public async Task SendStatisticsData()
        {
            var currentData = _statisticsService.GetCurrentStatistics();
            await Clients.All.SendAsync("ReceiveStatistics", currentData);
        }
    }
}