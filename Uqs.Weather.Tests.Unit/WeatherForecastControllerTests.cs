using AdamTibi.OpenWeather;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using Uqs.Weather.Controllers;

namespace Uqs.Weather.Tests.Unit;

public class WeatherForecastControllerTests
{
    [Theory]
    [InlineData(-100, -148)]
    [InlineData(-10.1, 13.8)]
    [InlineData(10, 50)]
    public void ConvertCToF_Cel_CorrectFah(double c, double f)
    {
        // Arrange
        NullLogger<WeatherForecastController> logger = NullLogger<WeatherForecastController>.Instance;
        WeatherForecastController controller = new WeatherForecastController(logger, null!, null!, null!, null!);

        // Act
        double actual = controller.ConvertCToF(c);

        // Assert
        Assert.Equal(f, actual, 1);
    }

    [Fact]
    public async Task GetReal_RequestsToOpenWeather_MetricUnitIsUsed()
    {
        IClient clientMock = Substitute.For<IClient>();
        await clientMock.OneCallAsync(
        Arg.Any<decimal>(),
        Arg.Any<decimal>(),
        Arg.Any<IEnumerable<Excludes>>(),
        Arg.Any<Units>());
        //.Returns(x => 
        // {
          //  OneCallResponse response = new OneCallResponse();
        //    return Task.FromResult(response);
       // });

        
    }
}