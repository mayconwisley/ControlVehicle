using ControlVehicle.Domain.Repositories;
using ControlVehicle.Infra.Database;
using ControlVehicle.Infra.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ControlVehicle.Infra;

public static class DependencyInjection
{
    public static IServiceCollection AddInfra(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<VehicleDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IDriverRepository, DriverRepository>();
        services.AddScoped<IVehicleRepository, VehicleRepository>();
        services.AddScoped<IVehicleControlRepository, VehicleControlRepository>();
        services.AddScoped<IFuelControlRepository, FuelControlRepository>();
        services.AddScoped<ITrafficFineControlRepository, TrafficFineControlRepository>();
        services.AddScoped<IMaintenanceControlRepository, MaintenanceControlRepository>();

        return services;
    }
}


