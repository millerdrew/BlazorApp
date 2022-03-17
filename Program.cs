using BlazorApp.Data;
using Tailwind;
using StackExchange.Redis;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);
string? getEnv = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
var multiplexer = ConnectionMultiplexer.Connect(getEnv == "Development" ? "localhost" : "redis");

var connString = getEnv == "Development" ? "Host=localhost;Username=postgres;Password=password;Database=postgres" :
                                                      "Host=postgres;Username=postgres;Password=password;Database=postgres" ;

await using var conn = new NpgsqlConnection(connString);
await conn.OpenAsync();

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddSingleton<IConnectionMultiplexer>(multiplexer);
builder.Services.AddSingleton<NpgsqlConnection>(conn);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // Tailwind hot reload
    // Ignore error, adding await will cause build to fail
    app.RunTailwind("tailwind", "./");
}

app.UseStaticFiles();

app.UseRouting();

app.MapGet("/hello", () => "Hello World!");

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();