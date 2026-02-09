using System.Text.Json;

namespace DotNetTodoApi.Services;

public class WeatherService : IWeatherService
{
    private static readonly string? WEATHER_API_URL = Environment.GetEnvironmentVariable("WEATHER_API_URL");
    private static readonly string? WEATHER_API_KEY = Environment.GetEnvironmentVariable("WEATHER_API_KEY");
    private static readonly HttpClient _httpClient = new HttpClient { };

    public async Task<JsonElement> FetchLocation(string query, string lang = "en", string units = "metric")
    {
        ValidateWeatherApiSettings();

        string apiUrl = $"{WEATHER_API_URL}?appid={WEATHER_API_KEY}&q={query}&lang={lang}&units={units}";

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
        if (string.IsNullOrEmpty(WEATHER_API_URL) || string.IsNullOrEmpty(WEATHER_API_KEY))
        {
            throw new Exception("WEATHER_API_URL and WEATHER_API_KEY must be set in the .env file.");
        }
    }
}