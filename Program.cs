using BlazorApp.Data;
using Tailwind;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);
Console.WriteLine("Update host for k8s cluster");
var multiplexer = ConnectionMultiplexer.Connect("redis");

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddSingleton<IConnectionMultiplexer>(multiplexer);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

// Tailwind hot reload
if (app.Environment.IsDevelopment())
{
    app.RunTailwind("tailwind", "./");
}

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();