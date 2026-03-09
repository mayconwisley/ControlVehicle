using ControlVehicle.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace ControlVehicle.Infra.Database;

public sealed class VehicleDbContext(DbContextOptions<VehicleDbContext> options) : DbContext(options)
{
	public DbSet<Vehicle> Vehicles => Set<Vehicle>();
	public DbSet<Driver> Drivers => Set<Driver>();
	public DbSet<DriverCnh> DriverCnhs => Set<DriverCnh>();

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.HasDefaultSchema("control_vehicle");
		modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
		base.OnModelCreating(modelBuilder);
	}
}
