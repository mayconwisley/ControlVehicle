using Asp.Versioning;
using ControlVehicle.App.Services.Driver;
using ControlVehicle.App.Services.Driver.Interface;
using ControlVehicle.App.Services.FuelControl;
using ControlVehicle.App.Services.FuelControl.Interface;
using ControlVehicle.App.Services.TrafficFineControl;
using ControlVehicle.App.Services.TrafficFineControl.Interface;
using ControlVehicle.App.Services.Vehicle;
using ControlVehicle.App.Services.Vehicle.Interface;
using ControlVehicle.App.Services.VehicleControl;
using ControlVehicle.App.Services.VehicleControl.Interface;
using ControlVehicle.Infra;
using Scalar.AspNetCore;
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

builder.Services.AddOpenApi("v1", v =>
{
    v.AddDocumentTransformer((document, context, ct) =>
    {
        document.Info = new()
        {
            Title = "Control Vehicle V1",
            Description = "Documentação do controle de veiculos",
            Version = "v1"
        };
        document.Servers = [
            new() {Url = "https://localhost:7096", Description = "Servidor Local"}
        ];
        document.ExternalDocs = new()
        {
            Description = "Acesse aqui para saber mais",
            Url = new Uri("https://github.com/mayconwisley/ControlVehicle")
        };
        return Task.CompletedTask;
    });
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
builder.Services.AddScoped<IVehicleControlServices, VehicleControlServices>();
builder.Services.AddScoped<IFuelControlServices, FuelControlServices>();
builder.Services.AddScoped<ITrafficFineControlServices, TrafficFineControlServices>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi("/doc/{documentName}.json");
    app.MapScalarApiReference("doc/scalar", options =>
    {
        options.ForceDarkMode();
        options
            .WithTitle("Controle Veiculo")
            .WithOpenApiRoutePattern("/doc/{documentName}.json")
            .AddDocument("v1", "Controle de Veiculo V1")
            .WithTheme(ScalarTheme.BluePlanet)
            .WithDefaultHttpClient(ScalarTarget.JavaScript, ScalarClient.Curl);

    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

