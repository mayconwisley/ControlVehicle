using ControlVehicle.Infra.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ControlVehicle.Infra;

public static class DependencyInjection
{
	public static IServiceCollection AddInfra(this IServiceCollection services, string connectionString)
	{
		services.AddDbContext<VehicleDbContext>(options =>
		   options.UseNpgsql(connectionString));

		return services;
	}
}
