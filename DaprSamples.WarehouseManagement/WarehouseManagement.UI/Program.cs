using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Serilog;
using Serilog.Formatting.Json;


Log.Logger = new LoggerConfiguration()
    .WriteTo.Console(new JsonFormatter())
    .WriteTo.Seq("http://seq:5341")
    .MinimumLevel.Debug()
    .CreateBootstrapLogger();

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddDaprClient();
builder.Services.AddRazorPages();
//builder.Services.AddServerSideBlazor();
//builder.Services.AddSingleton<WeatherForecastService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}


app.UseSerilogRequestLogging();
app.UseStaticFiles();

app.UseRouting();


app.MapRazorPages();
//app.MapBlazorHub();
//app.MapFallbackToPage("/_Host");

app.Run();
