using ControlVehicle.App.Services.Driver;
using ControlVehicle.App.Services.Driver.Interface;
using ControlVehicle.App.Services.Vehicle;
using ControlVehicle.App.Services.Vehicle.Interface;
using ControlVehicle.Infra;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var passDatabase = Environment.GetEnvironmentVariable("SQLSenha", EnvironmentVariableTarget.Machine);
if (string.IsNullOrWhiteSpace(passDatabase))
	throw new InvalidOperationException("Variavel de ambiente 'SQLSenha' nao encontrada (Machine).");

var connectionString = builder.Configuration.GetConnectionString("VehicleConnection")!;
var csb = new Npgsql.NpgsqlConnectionStringBuilder(connectionString)
{
	Password = passDatabase
};

builder.Services.AddInfra(csb.ConnectionString);
builder.Services.AddScoped<IDriverServices, DriverServices>();
builder.Services.AddScoped<IVehicleServices, VehicleServices>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
