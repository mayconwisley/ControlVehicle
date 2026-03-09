using Asp.Versioning;
using ControlVehicle.App.Services.Driver;
using ControlVehicle.App.Services.Driver.Interface;
using ControlVehicle.App.Services.DriverCnh;
using ControlVehicle.App.Services.DriverCnh.Interface;
using ControlVehicle.App.Services.Vehicle;
using ControlVehicle.App.Services.Vehicle.Interface;
using ControlVehicle.Infra;
using Microsoft.OpenApi;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services
	.AddControllers()
	.AddJsonOptions(options =>
	{
		options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
	});

builder.Services.AddApiVersioning(options =>
{
	options.DefaultApiVersion = new ApiVersion(1, 0);
	options.AssumeDefaultVersionWhenUnspecified = true;
	options.ReportApiVersions = true;
	options.ApiVersionReader = new UrlSegmentApiVersionReader();
}).AddApiExplorer(options =>
{
	options.GroupNameFormat = "'v'VVV";
	options.SubstituteApiVersionInUrl = true;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
	options.SwaggerDoc("v1", new OpenApiInfo
	{
		Title = "ControlVehicle API",
		Version = "v1"
	});
	options.DocInclusionPredicate((docName, apiDesc) => apiDesc.GroupName == docName);
	options.UseInlineDefinitionsForEnums();
});

var passDatabase = Environment.GetEnvironmentVariable("SQLPassword", EnvironmentVariableTarget.Machine);
if (string.IsNullOrWhiteSpace(passDatabase))
	throw new InvalidOperationException("Variavel de ambiente 'SQLPassword' nao encontrada (Machine).");

var connectionString = builder.Configuration.GetConnectionString("VehicleConnection")!;
var csb = new Npgsql.NpgsqlConnectionStringBuilder(connectionString)
{
	Password = passDatabase
};

builder.Services.AddInfra(csb.ConnectionString);
builder.Services.AddScoped<IDriverServices, DriverServices>();
builder.Services.AddScoped<IVehicleServices, VehicleServices>();
builder.Services.AddScoped<IDriverCnhServices, DriverCnhServices>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI(options =>
	{
		options.SwaggerEndpoint("/swagger/v1/swagger.json", "ControlVehicle API v1");
	});
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
