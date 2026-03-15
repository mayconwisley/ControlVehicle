using ControlVehicle.Models.Dtos;

namespace ControlVehicle.App.Services.Vehicle.Interface;

public interface IVehicleServices
{
	public Task<IEnumerable<VehicleDto>> GetAll(int page, int size, string search);
	public Task<VehicleDto?> GetByPlate(string plate);
	public Task<VehicleDto?> GetByRenavam(string renavam);
	public Task<VehicleDto> Create(VehicleCreateDto vehicle);
	public Task<VehicleDto?> Update(VehicleUpdateDto vehicle);
	public Task Delete(string renavam);
	public Task<int> TotalVehicle();
}
