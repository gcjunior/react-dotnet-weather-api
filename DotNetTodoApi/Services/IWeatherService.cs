using System.Text.Json;

namespace DotNetTodoApi.Services;

// service interface
public interface IWeatherService
{
    public Task<JsonElement> FetchLocation(string query, string lang = "en", string units = "metric");
}
