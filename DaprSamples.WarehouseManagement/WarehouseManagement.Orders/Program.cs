
using FluentValidation;
using MediatR;
using Serilog;
using Serilog.Formatting.Json;
using System.Reflection;
using WarehouseManagement.Customers.Validations;
using WarehouseManagement.Orders;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console(new JsonFormatter())
    .WriteTo.Seq("http://seq:5341")
    .MinimumLevel.Debug()
    .CreateBootstrapLogger();

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog();

// Add services to the container.

builder.Services.AddDaprClient();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddValidatorsFromAssemblyContaining<CustomerCreatedEventValidator>();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IInventoryService, InventoryService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddAutoMapper(typeof(Program));


var app = builder.Build();
app.MapSubscribeHandler();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseSerilogRequestLogging();
app.UseCloudEvents();
app.UseAuthorization();

app.MapControllers();

app.Run();
