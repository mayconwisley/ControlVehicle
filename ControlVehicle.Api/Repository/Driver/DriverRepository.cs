using ControlVehicle.Api.Database;
using ControlVehicle.Api.Models.Driver;
using ControlVehicle.Api.Repository.Driver.Interface;
using Microsoft.EntityFrameworkCore;

namespace ControlVehicle.Api.Repository.Driver;

public class DriverRepository(VehicleDbContext vehicleDbContext) : IDriverRepository
{
    private readonly VehicleDbContext _vehicleDbContext = vehicleDbContext;

    public async Task<IEnumerable<DriverModel>> GetAll(int page, int size, string search)
    {
        var driverList = await _vehicleDbContext.Drivers
            .Skip((page - 1) * size)
            .Take(size)
            .OrderBy(o => o.Name)
            .ToListAsync();

        return driverList;

    }

    public async Task<DriverModel> GetByCnh(string cnh)
    {
        var driver = await _vehicleDbContext.Drivers
            .Where(w => w.CNH == cnh)
            .FirstOrDefaultAsync();
        if (driver is not null)
        {
            return driver;
        }
        return new();
    }

    public async Task<DriverModel> Create(DriverModel driver)
    {
        if (driver is not null)
        {
            _vehicleDbContext.Drivers.Add(driver);
            await _vehicleDbContext.SaveChangesAsync();
            return driver;
        }
        return new();
    }

    public async Task<DriverModel> Update(DriverModel driver)
    {
        if (driver is not null)
        {
            _vehicleDbContext.Drivers.Entry(driver).State = EntityState.Modified;
            await _vehicleDbContext.SaveChangesAsync();
            return driver;
        }
        return new();
    }

    public async Task<DriverModel> Delete(string cnh)
    {
        var driver = await GetByCnh(cnh);
        if (driver is not null)
        {
            _vehicleDbContext.Drivers.Remove(driver);
            await _vehicleDbContext.SaveChangesAsync();
            return driver;
        }
        return new();
    }


    public async Task<int> TotalDriver()
    {
        int totalDriver = await _vehicleDbContext.Drivers.CountAsync();
        return totalDriver;
    }


}
