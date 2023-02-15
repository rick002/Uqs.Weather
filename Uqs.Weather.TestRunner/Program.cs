
using Microsoft.Extensions.Logging.Abstractions;
using Uqs.Weather.Controllers;

NullLogger<WeatherForecastController> logger = NullLogger<WeatherForecastController>.Instance;

// Fails

var controller = new WeatherForecastController(logger, null!, null!, null!, null!);

double f1 = controller.ConvertCToF(-1.0);

if (f1 != 30.20d)
{
    throw new System.Exception("Invalid");
}

double f2 = controller.ConvertCToF(1.2);

if (f2 != 34.16d)
{
    throw new System.Exception("Invalid");
}

Console.WriteLine("Test Passed");