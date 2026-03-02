using ControlVehicle.Infra;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Connection string
var passDatabase = Environment.GetEnvironmentVariable("SQLSenha", EnvironmentVariableTarget.Machine);

if (string.IsNullOrWhiteSpace(passDatabase))
	throw new InvalidOperationException("Variável de ambiente 'SQLSenha' não encontrada (Machine).");

var connectionString = builder.Configuration
	.GetConnectionString("VehicleConnection")!;

// Se o appsettings já tem Password=, dá pra só substituir o final:
connectionString = connectionString + passDatabase;
// (melhor opção abaixo 👇)

// ✅ Recomendo NpgsqlConnectionStringBuilder (mais seguro)
var csb = new Npgsql.NpgsqlConnectionStringBuilder(connectionString)
{
	Password = passDatabase
};

builder.Services.AddInfra(csb.ConnectionString);


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
