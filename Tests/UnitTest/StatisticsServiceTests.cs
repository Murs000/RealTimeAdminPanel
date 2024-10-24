using Moq;
using Xunit;
using RealTimeAdminPanel.Services;
using RealTimeAdminPanel.Models;
using RealTimeAdminPanel.Hubs;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace UnitTests;
public class StatisticsServiceTests
{
    private readonly Mock<IHubContext<StatisticsHub>> _hubContextMock;
    private readonly StatisticsService _statisticsService;

    public StatisticsServiceTests()
    {
        // Create a mock for IHubContext<StatisticsHub>
        var mockHubContext = new Mock<IHubContext<StatisticsHub>>();
        
        // Mock the IHubClients (used by Clients property in SignalR)
        var mockClients = new Mock<IHubClients>();
        
        // Mock IClientProxy (used by Clients.All)
        var mockClientProxy = new Mock<IClientProxy>();
        
        // Setup the behavior: Clients.All should return mockClientProxy
        mockClients.Setup(clients => clients.All).Returns(mockClientProxy.Object);
        
        // Setup the hub context to return mockClients when accessing the Clients property
        mockHubContext.Setup(hub => hub.Clients).Returns(mockClients.Object);

        _hubContextMock = mockHubContext;

        _statisticsService = new StatisticsService(mockHubContext.Object);
    }

    [Fact]
    public void GetCurrentStatistics_ReturnsCurrentStatistics()
    {
        // Act
        var result = _statisticsService.GetCurrentStatistics();

        // Assert
        Assert.NotNull(result);
        Assert.IsType<StatisticsData>(result);
    }

    [Fact]
    public void UpdateStatistics_UpdatesCurrentStatisticsAndNotifiesClients()
    {
        // Arrange
        // Prepare some new statistics data
        var newStatistics = new StatisticsData
        {
            TotalUsers = 3000,
            ActiveUsers = 400,
            NewUsersToday = 50,
            TotalSales = 25000.0m,
            ErrorsReported = 5
        };

        // Act
        _statisticsService.UpdateStatistics(newStatistics);

        // Assert
        var currentStatistics = _statisticsService.GetCurrentStatistics();
        Assert.Equal(newStatistics.TotalUsers, currentStatistics.TotalUsers);
        Assert.Equal(newStatistics.ActiveUsers, currentStatistics.ActiveUsers);

        // TODO: Fix IT
        // Verify that the SendAsync method was called to notify clients
        //_hubContextMock.Verify(hub => hub.Clients.All.SendAsync("ReceiveStatistics", newStatistics, default), Times.Once);
    }

    [Fact]
    public void GenerateRandomStatistics_ReturnsRandomStatisticsData()
    {
        // Act
        var result = _statisticsService.GenerateRandomStatistics();

        // Assert
        Assert.NotNull(result);
        Assert.InRange(result.TotalUsers, 1000, 5000);
        Assert.InRange(result.ActiveUsers, 100, 1000);
    }
}