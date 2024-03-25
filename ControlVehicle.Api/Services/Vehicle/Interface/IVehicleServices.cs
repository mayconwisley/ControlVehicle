using ControlVehicle.Dto.Vehicle;

namespace ControlVehicle.Api.Services.Vehicle.Interface;

public interface IVehicleServices
{
    public Task<IEnumerable<VehicleDto>> GetAll(int page, int size, string search);
    public Task<VehicleDto> GetByPlate(string plate);
    public Task<VehicleDto> GetByRenavam(string renavam);
    public Task Create(VehicleDto vehicle);
    public Task Update(VehicleDto vehicle);
    public Task Delete(string renavam);
    public Task<int> TotalVehicle();
}
