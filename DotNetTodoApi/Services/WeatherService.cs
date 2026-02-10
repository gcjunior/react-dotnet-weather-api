using System.Text.Json;
using Microsoft.Extensions.Options;

namespace DotNetTodoApi.Services;

public class WeatherService : IWeatherService
{
    private readonly HttpClient _httpClient;
    private readonly OpenWeatherOptions _options;

    public WeatherService(HttpClient httpClient, IOptions<OpenWeatherOptions> options)
    {
        _httpClient = httpClient;
        _options = options.Value;
    }

    public async Task<JsonElement> FetchLocation(string query, string lang = "en", string units = "metric")
    {
        ValidateWeatherApiSettings();

        string apiUrl = $"weather/?appid={_options.ApiKey}&q={query}&lang={lang}&units={units}";

        var response = await _httpClient.GetAsync(apiUrl);

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<JsonElement>();
            return result!;
        }

        throw new HttpRequestException($"Weather API request failed with status code {response.StatusCode}");
    }

    private void ValidateWeatherApiSettings()
    {
        if (string.IsNullOrEmpty(_httpClient.BaseAddress?.ToString()) || string.IsNullOrEmpty(_options.ApiKey))
        {
            throw new Exception("WEATHER_API_URL and WEATHER_API_KEY must be set.");
        }
    }
}
