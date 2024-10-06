using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using RealTimeAdminPanel.Hubs;
using RealTimeAdminPanel.Models;
using RealTimeAdminPanel.Services;

namespace RealTimeAdminPanel.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatisticsController : ControllerBase
    {
        private readonly StatisticsService _statisticsService;
        private readonly IHubContext<StatisticsHub> _hubContext;

        public StatisticsController(StatisticsService statisticsService, IHubContext<StatisticsHub> hubContext)
        {
            _statisticsService = statisticsService;
            _hubContext = hubContext;
        }

        // Get the current statistics
        [HttpGet("current")]
        public IActionResult GetCurrentStatistics()
        {
            var data = _statisticsService.GetCurrentStatistics();
            return Ok(data);
        }

        // Update statistics data
        [HttpPut("update")]
        public IActionResult UpdateStatistics([FromBody] StatisticsData newData)
        {
            _statisticsService.UpdateStatistics(newData);

            return Ok(new { Message = "Statistics updated successfully!" });
        }

        // Reset the statistics to random values
        [HttpPost("reset")]
        public IActionResult ResetStatistics()
        {
            var newData = _statisticsService.GenerateRandomStatistics();
            _statisticsService.UpdateStatistics(newData);

            return Ok(newData);
        }
    }
}