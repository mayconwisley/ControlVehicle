using ControlVehicle.Api.Models.Driver;

namespace ControlVehicle.Api.Repository.Driver.Interface;

public interface IDriverRepository
{
    public Task<IEnumerable<DriverModel>> GetAll(int page, int size, string search);
    public Task<DriverModel> GetByCnh(string cnh);
    public Task<DriverModel> Create(DriverModel driver);
    public Task<DriverModel> Update(DriverModel driver);
    public Task<DriverModel> Delete(string cnh);
    public Task<int> TotalDriver();
}
