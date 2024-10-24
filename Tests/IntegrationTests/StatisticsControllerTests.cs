using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTests;

public class StatisticsControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>> // Use your main entry class (Program)
{
    private readonly HttpClient _client;

    public StatisticsControllerIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient(); // Creates an in-memory version of the app
    }

    [Fact]
    public async Task GetCurrentStatistics_ShouldReturnOkResult()
    {
        // Act
        var response = await _client.GetAsync("/api/Statistics/current");

        // Assert
        response.EnsureSuccessStatusCode(); // Verifies that the status code is 200 (OK)
        var responseBody = await response.Content.ReadAsStringAsync();
        Assert.NotNull(responseBody);
    }

    [Fact]
    public async Task UpdateStatistics_ShouldReturnSuccessMessage()
    {
        // Arrange
        var newStatistics = new
        {
            TotalUsers = 2000,
            ActiveUsers = 300,
            NewUsersToday = 30,
            TotalSales = 15000.5,
            ErrorsReported = 2
        };

        var content = new StringContent(JsonSerializer.Serialize(newStatistics), System.Text.Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PutAsync("/api/Statistics/update", content);

        // Assert
        response.EnsureSuccessStatusCode(); // Verifies the status code is 200 (OK)
        var responseBody = await response.Content.ReadAsStringAsync();
        Assert.Contains("Statistics updated successfully!", responseBody);
    }

    [Fact]
    public async Task SwaggerEndpoint_ShouldBeAvailable()
    {
        // Act
        var response = await _client.GetAsync("/swagger/index.html");

        // Assert
        response.EnsureSuccessStatusCode(); // Verifies that Swagger UI is reachable
    }
}