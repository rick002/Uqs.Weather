using Microsoft.AspNetCore.Mvc;
using AdamTibi.OpenWeather;
using Uqs.Weather.Wrapper;

namespace Uqs.Weather.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly int FORECAST_DAYS = 5;
    private readonly decimal GREENWICH_LAT = 51.477928M;
    private readonly decimal GREENWICH_LON = -0.001545M;

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IConfiguration _config;
    private readonly IClient _client;
    private readonly INowWrapper _nowWrapper;
    private readonly IRandomWrapper _randomWrapper;

    public WeatherForecastController(
        ILogger<WeatherForecastController> logger,
        IConfiguration config,
        IClient client,
        INowWrapper nowWrapper,
        IRandomWrapper randomWrapper)
    {
        _logger = logger;
        _config = config;
        _client = client;
        _nowWrapper = nowWrapper;
        _randomWrapper = randomWrapper;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        WeatherForecast[] forecast = new WeatherForecast[FORECAST_DAYS];

        for (int i = 0; i < forecast.Length; i++)
        {
            WeatherForecast weatherForecast = forecast[i] = new WeatherForecast();
            weatherForecast.Date = DateTime.Now.AddDays(i + 1);
            weatherForecast.TemperatureC = _randomWrapper.Next(-20, 55);
            weatherForecast.Summary = MapFeelToTemp(weatherForecast.TemperatureC);
        }

        return forecast;
    }

    [HttpGet("GetRealWeatherForecast")]
    public async Task<OneCallResponse> GetReal()
    {
        string apiKey = _config["OpenWeather:Key"];
        HttpClient httpClient = new HttpClient();

        OneCallResponse response = await _client.OneCallAsync(GREENWICH_LAT, GREENWICH_LON, new [] {
            Excludes.Current, Excludes.Alerts, 
            Excludes.Minutely, Excludes.Hourly,
        }, Units.Metric);

        return response;
    }

    [HttpGet("ConvertCToF")]
    public double ConvertCToF(double c)
    {
        double f = c * (9d / 5d) + 32;
        _logger.LogInformation("conversion requested");
        return f;
    }

    private string MapFeelToTemp(int temperatureC)
    {
        if (temperatureC <= 0)
        {
            return Summaries.First();
        }

        int summariesIndex = (temperatureC / 5) + 1;

        if (summariesIndex >= Summaries.Length)
        {
            return Summaries.Last();
        }

        return Summaries[summariesIndex];
    }
}
