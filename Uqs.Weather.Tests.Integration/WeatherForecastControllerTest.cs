using System.Net.Http.Json;

namespace Uqs.Weather.Tests.Integration;

public class WeatherForecastControllerTest
{
    private const string BASE_ADDRESS = "";
    private const string API_URI = "/WeatherForecast/GetRealWeatherForecast";
    private record WeatherForecast(DateTime Date, int TemperatureC, int TemperatureF, string? Summary);
    
    [Fact]
    public async Task GetRealWeatherForecast_Execute_GetNext5Days()
    {
        // Arrange
        HttpClient httpClient = new HttpClient { BaseAddress = new Uri(BASE_ADDRESS) };
        DateTime today = DateTime.Now.Date;
        DateTime[] next5Days = new [] { 
            today.AddDays(1) , 
            today.AddDays(2), 
            today.AddDays(3), 
            today.AddDays(4), 
            today.AddDays(5) 
        };
        
        // Act
        HttpResponseMessage response = await httpClient.GetAsync(API_URI);

        // Assert
        WeatherForecast[] forecast = await response.Content.ReadFromJsonAsync<WeatherForecast[]>() ?? new WeatherForecast[0];
        
        for (int i = 0; i < 5; i++)
        {
            Assert.Equal(next5Days[i], forecast[i].Date.Date);
        }
        
        
    }
}