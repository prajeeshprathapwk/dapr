

using FluentValidation;
using MediatR;
using Serilog;
using Serilog.Formatting.Json;
using System.Reflection;
using WarehouseManagement.Inventory;
using WarehouseManagement.Inventory.Validations;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console(new JsonFormatter())
    .WriteTo.Seq("http://seq:5341")
    .MinimumLevel.Debug()
    .CreateBootstrapLogger();

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog();

builder.Services.AddDaprClient();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddValidatorsFromAssemblyContaining<InventoryValidator>();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
builder.Services.AddScoped<IInventoryService, InventoryService>();
builder.Services.AddAutoMapper(typeof(Program));

var app = builder.Build();

app.MapSubscribeHandler();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseSerilogRequestLogging();
app.UseCloudEvents();

app.UseAuthorization();

app.MapControllers();

app.Run();
