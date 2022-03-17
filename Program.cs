using BlazorApp.Data;
using Tailwind;
using StackExchange.Redis;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);
Console.WriteLine("Update host for k8s cluster");
var multiplexer = ConnectionMultiplexer.Connect("redis");

var connString = "Host=postgres;Username=postgres;Password=password;Database=postgres";

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
}

// Tailwind hot reload
if (app.Environment.IsDevelopment())
{
    // Ignore error, adding await will cause build to fail
    app.RunTailwind("tailwind", "./");
}

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();