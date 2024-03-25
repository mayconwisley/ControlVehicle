using ControlVehicle.Api.Database;
using ControlVehicle.Api.Repository.Driver;
using ControlVehicle.Api.Repository.Driver.Interface;
using ControlVehicle.Api.Repository.Vehicle;
using ControlVehicle.Api.Repository.Vehicle.Interface;
using ControlVehicle.Api.Services.Driver;
using ControlVehicle.Api.Services.Driver.Interface;
using ControlVehicle.Api.Services.Vehicle;
using ControlVehicle.Api.Services.Vehicle.Interface;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database //////////////////////////////
var passDatabase = Environment.GetEnvironmentVariable("SQLSenha", EnvironmentVariableTarget.Machine);
string connectionDatabase = builder.Configuration.GetConnectionString("VehicleConnection")!.Replace("{{pass}}", passDatabase);
builder.Services.AddDbContext<VehicleDbContext>(x => x.UseSqlServer(connectionDatabase));
//////////////////////////////////////////

builder.Services.AddScoped<IDriverRepository, DriverRepository>();
builder.Services.AddScoped<IDriverServices, DriverServices>();
builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();
builder.Services.AddScoped<IVehicleServices, VehicleServices>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
