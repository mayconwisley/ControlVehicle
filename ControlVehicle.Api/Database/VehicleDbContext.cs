using ControlVehicle.Api.Models.Driver;
using ControlVehicle.Api.Models.Vehicle;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace ControlVehicle.Api.Database;

public class VehicleDbContext(DbContextOptions<VehicleDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public DbSet<VehicleModel> Vehicles { get; set; }
    public DbSet<DriverModel> Drivers { get; set; }
}
