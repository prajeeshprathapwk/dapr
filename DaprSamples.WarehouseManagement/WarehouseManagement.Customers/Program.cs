
using FluentValidation;
using MediatR;
using Serilog;
using Serilog.Formatting.Json;
using System.Reflection;
using WarehouseManagement.Customers;
using WarehouseManagement.Customers.Validations;

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

builder.Services.AddValidatorsFromAssemblyContaining<CustomerValidator>();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddAutoMapper(typeof(Program));

var app = builder.Build();

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
