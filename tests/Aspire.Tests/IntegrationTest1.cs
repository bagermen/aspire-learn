namespace Aspire.Tests.Tests;

public class IntegrationTest1(TestingAspireAppHost host) : IClassFixture<TestingAspireAppHost>
{
    [Fact]
    public async Task GetWeatherforecastErrortatusCode()
    {
        // Act
        var httpClient = host.CreateHttpClient("aspire-api2", "https");
        var response = await httpClient.GetAsync("/weatherforecast");

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
