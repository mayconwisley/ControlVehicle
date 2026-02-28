using ControlVehicle.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace ControlVehicle.Infra.Database;

public sealed class VehicleDbContext(DbContextOptions<VehicleDbContext> options) : DbContext(options)
{
	public DbSet<Vehicle> Vehicles => Set<Vehicle>();
	public DbSet<Driver> Drivers => Set<Driver>();

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		// 1) Schema (evite camelCase em Postgres se possível)
		modelBuilder.HasDefaultSchema("control_vehicle");

		// 2) Aplica as IEntityTypeConfiguration<>
		modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

		base.OnModelCreating(modelBuilder);
	}
}