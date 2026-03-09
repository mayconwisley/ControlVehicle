using ControlVehicle.Infra.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ControlVehicle.Infra;

public sealed class VehicleDbContextFactory : IDesignTimeDbContextFactory<VehicleDbContext>
{
	public VehicleDbContext CreateDbContext(string[] args)
	{
		var explicitConnection = Environment.GetEnvironmentVariable("EF_CONNECTION_STRING");
		if (!string.IsNullOrWhiteSpace(explicitConnection))
		{
			var explicitOptions = new DbContextOptionsBuilder<VehicleDbContext>();
			explicitOptions.UseNpgsql(explicitConnection);
			return new VehicleDbContext(explicitOptions.Options);
		}

		var password = Environment.GetEnvironmentVariable("SQLPassword")
			?? Environment.GetEnvironmentVariable("SQLPassword", EnvironmentVariableTarget.Machine)
			?? string.Empty;

		var connectionString = $"Host=localhost;Port=5432;Database=ParentTreeDB;Username=postgres;Password={password}";
		var options = new DbContextOptionsBuilder<VehicleDbContext>();
		options.UseNpgsql(connectionString);

		return new VehicleDbContext(options.Options);
	}
}

