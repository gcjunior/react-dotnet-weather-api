using System.Text.Json;
using DotNetTodoApi.Services;
using DotNetEnv;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
    Console.WriteLine($"Running in development.");
}
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

builder.Services.AddSingleton<IWeatherService, WeatherService>();

builder.Services.AddHttpClient();

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