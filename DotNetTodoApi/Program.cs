using System.Text.Json;
using DotNetTodoApi.Services;
using DotNetEnv;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
    Console.WriteLine($"Running in development.");
}
// OpenWeather DI setup
builder.Services.Configure<OpenWeatherOptions>(builder.Configuration.GetSection("OpenWeatherMap"));
builder.Services.AddHttpClient<IWeatherService, WeatherService>((sp, client) =>
{
    var options = sp.GetRequiredService<IOptions<OpenWeatherOptions>>().Value;
    client.BaseAddress = new Uri(
           $"{options.BaseUrl}{options.Version}/");
});

builder.Services.AddControllers(options =>
{
    options.RespectBrowserAcceptHeader = true;
})
    .AddJsonOptions(options =>
    {
        // Set to true for pretty printing
        options.JsonSerializerOptions.WriteIndented = true;

        // Optional: Keep property names as camelCase (default)
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    });

// builder.Services.AddSingleton<IWeatherService, WeatherService>(); // Registers WeatherService for DI, but we need to configure HttpClient for it, so we use AddHttpClient with a factory method above.
// builder.Services.AddHttpClient(); // Registers HttpClient for DI, but we need to configure it for WeatherService, so we use AddHttpClient with a factory method above.

var app = builder.Build();

app.UseCors(cors => cors
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/oops");
}

Env.Load(); // loads .env into Environment variables

app.MapGet("/oops", () => "Oops! An error happened.");

app.MapControllers(); // Maps the attribute routes from the controllers

app.Run();
