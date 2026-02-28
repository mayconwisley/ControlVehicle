using ControlVehicle.Domain.Entities;

namespace ControlVehicle.Domain.Repositories;

public interface IDriverRepository
{
    public Task<IEnumerable<Driver>> GetAll(int page, int size, string search);
    public Task<Driver> GetByCnh(string cnh);
    public Task<Driver> Create(Driver driver);
    public Task<Driver> Update(Driver driver);
    public Task<Driver> Delete(string cnh);
    public Task<int> TotalDriver();
}
