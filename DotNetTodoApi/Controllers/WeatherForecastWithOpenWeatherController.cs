using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using DotNetTodoApi.Services;

[Route("api/[controller]")]
[ApiController]
public class WeatherForecastWithOpenWeatherController : ControllerBase
{
    private readonly ILogger<WeatherForecastWithOpenWeatherController> _logger;
    // Replace with your actual OpenWeather API key
    // private string API_KEY = "dbdbb3c694a1dbe2d46ded25eca64646";
    // private string BASE_URL = "https://api.openweathermap.org/data/2.5/weather";

    private readonly IWeatherService _weatherService;

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
        using var httpClient = new HttpClient();
        // var searchQuery = $"{BASE_URL}?q={cityName}";
        var searchQuery = $"{cityName}";
        if (!string.IsNullOrEmpty(stateCode) && !string.IsNullOrEmpty(countryCode))
        {
            searchQuery += $",{stateCode},{countryCode}";
        }
        // searchQuery += $"&appid={API_KEY}&units=metric";
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

        // var response = await httpClient.GetAsync(url);
        // if (response.IsSuccessStatusCode)
        // {
        //     var content = await response.Content.ReadFromJsonAsync<JsonElement>();
        //     return Ok(content);
        // }
        // return StatusCode((int)response.StatusCode, "Error fetching weather data");
    }
}
