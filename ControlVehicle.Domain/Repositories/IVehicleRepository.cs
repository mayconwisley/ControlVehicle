using ControlVehicle.Domain.Entities;

namespace ControlVehicle.Domain.Repositories;

public interface IVehicleRepository
{
    public Task<IEnumerable<Vehicle>> GetAll(int page, int size, string search);
    public Task<Vehicle> GetByPlate(string plate);
    public Task<Vehicle> GetByRenavam(string renavam);
    public Task<Vehicle> Create(Vehicle driver);
    public Task<Vehicle> Update(Vehicle driver);
    public Task<Vehicle> Delete(string renavam);
    public Task<int> TotalVehicle();
}
