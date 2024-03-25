using ControlVehicle.Api.Models.Vehicle;

namespace ControlVehicle.Api.Repository.Vehicle.Interface;

public interface IVehicleRepository
{
    public Task<IEnumerable<VehicleModel>> GetAll(int page, int size, string search);
    public Task<VehicleModel> GetByPlate(string plate);
    public Task<VehicleModel> GetByRenavam(string renavam);
    public Task<VehicleModel> Create(VehicleModel driver);
    public Task<VehicleModel> Update(VehicleModel driver);
    public Task<VehicleModel> Delete(string renavam);
    public Task<int> TotalVehicle();
}
