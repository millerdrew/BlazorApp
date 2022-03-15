using BlazorApp.Data;
using Tailwind;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();

// Add Redis
ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("localhost");
IDatabase db = redis.GetDatabase();
string value1 = "abcdefg";
db.StringSet("mykey", value1);
string value2 = db.StringGet("mykey");
Console.WriteLine(value2); // writes: "abcdefg"

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