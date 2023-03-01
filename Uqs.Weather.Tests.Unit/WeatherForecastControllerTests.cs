using AdamTibi.OpenWeather;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using Uqs.Weather.Controllers;
using Uqs.Weather.Wrapper;

namespace Uqs.Weather.Tests.Unit;

public class WeatherForecastControllerTests
{
    private const double NEXT_T = 3.3;
    private const double DAY5_T = 7.7;
    private readonly DateTime _today = new(2022, 1, 1);
    private readonly double[] _realWeatherTemps = new[] { 2, NEXT_T, 4, 5.5, 6, DAY5_T, 8 };
    private readonly ILogger<WeatherForecastController> _loggerMock = Substitute.For<ILogger<WeatherForecastController>>();
    private readonly INowWrapper _nowWrapper = Substitute.For<INowWrapper>();
    private readonly IRandomWrapper _randomWrapperMock = Substitute.For<IRandomWrapper>();
    private readonly IClient _clientMock = Substitute.For<IClient>();
    private readonly WeatherForecastController _sut;

    public WeatherForecastControllerTests()
    {
        _sut = new WeatherForecastController(_loggerMock, null!, _clientMock, _nowWrapper, _randomWrapperMock);
    }

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
    public void GetReal_RequestsToOpenWeather_MetricUnitIsUsed()
    {
        // Arrange

        OneCallResponse res = new OneCallResponseBuilder()
            .SetTemps(new[] { 0, 3.3, 0, 0, 0, 0, 0 })
            .Build();
        // Act
        
        _clientMock.OneCallAsync(Arg.Any<decimal>(),
            Arg.Any<decimal>(),
            Arg.Any<IEnumerable<Excludes>>(),
            Arg.Any<Units>())

        // Assert
        .Returns(res);
    }
}