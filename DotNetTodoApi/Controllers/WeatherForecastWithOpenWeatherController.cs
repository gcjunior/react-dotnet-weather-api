using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using DotNetTodoApi.Services;

[Route("api/[controller]")]
[ApiController]
public class WeatherForecastWithOpenWeatherController : ControllerBase
{
    private readonly IWeatherService _weatherService;
    private readonly ILogger<WeatherForecastWithOpenWeatherController> _logger;

    public WeatherForecastWithOpenWeatherController(ILogger<WeatherForecastWithOpenWeatherController> logger, IWeatherService weatherService)
    {
        _logger = logger;
        _weatherService = weatherService;
    }

    // Get weather by city name
    [HttpGet("city/{cityName}")]
    [HttpGet("city/{cityName}/{stateCode}/{countryCode}")]
    public async Task<IActionResult> GetWeatherByCity(string cityName, string? stateCode = null, string? countryCode = null)
    {
        var searchQuery = $"{cityName}";
        if (!string.IsNullOrEmpty(stateCode) && !string.IsNullOrEmpty(countryCode))
        {
            searchQuery += $",{stateCode},{countryCode}";
        }
        try
        {
            var result = await _weatherService.FetchLocation(searchQuery);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching weather data");
            return StatusCode(500, "Internal server error");
        }
    }
}
