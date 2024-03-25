using ControlVehicle.Api.Database;
using ControlVehicle.Api.Models.Vehicle;
using ControlVehicle.Api.Repository.Vehicle.Interface;
using Microsoft.EntityFrameworkCore;

namespace ControlVehicle.Api.Repository.Vehicle;

public class VehicleRepository(VehicleDbContext vehicleDbContext) : IVehicleRepository
{
    private readonly VehicleDbContext _vehicleDbContext = vehicleDbContext;
    public async Task<IEnumerable<VehicleModel>> GetAll(int page, int size, string search)
    {
        var vehicleList = await _vehicleDbContext.Vehicles
           .Skip((page - 1) * size)
           .Take(size)
           .OrderBy(o => o.Plate)
           .ToListAsync();

        return vehicleList;
    }

    public async Task<VehicleModel> GetByPlate(string plate)
    {
        var vehicle = await _vehicleDbContext.Vehicles
           .Where(w => w.Plate == plate)
           .FirstOrDefaultAsync();
        if (vehicle is not null)
        {
            return vehicle;
        }
        return new();
    }

    public async Task<VehicleModel> GetByRenavam(string renavam)
    {
        var vehicle = await _vehicleDbContext.Vehicles
           .Where(w => w.Renavam == renavam)
           .FirstOrDefaultAsync();
        if (vehicle is not null)
        {
            return vehicle;
        }
        return new();
    }
    public async Task<VehicleModel> Create(VehicleModel vehicle)
    {
        if (vehicle is not null)
        {
            _vehicleDbContext.Vehicles.Add(vehicle);
            await _vehicleDbContext.SaveChangesAsync();
            return vehicle;
        }
        return new();
    }
    public async Task<VehicleModel> Update(VehicleModel vehicle)
    {
        if (vehicle is not null)
        {
            _vehicleDbContext.Vehicles.Entry(vehicle).State = EntityState.Modified;
            await _vehicleDbContext.SaveChangesAsync();
            return vehicle;
        }
        return new();
    }
    public async Task<VehicleModel> Delete(string renavam)
    {
        var vehicle = await GetByRenavam(renavam);
        if (vehicle is not null)
        {
            _vehicleDbContext.Vehicles.Remove(vehicle);
            await _vehicleDbContext.SaveChangesAsync();
            return vehicle;
        }
        return new();
    }

    public async Task<int> TotalVehicle()
    {
        int totalVehicle = await _vehicleDbContext.Vehicles.CountAsync();
        return totalVehicle;
    }


}
