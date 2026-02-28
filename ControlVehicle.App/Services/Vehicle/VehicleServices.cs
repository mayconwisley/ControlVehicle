using ControlVehicle.Api.MappingDto.Vehicle;
using ControlVehicle.Api.Repository.Vehicle.Interface;
using ControlVehicle.Api.Services.Vehicle.Interface;
using ControlVehicle.Dto.Vehicle;

namespace ControlVehicle.Api.Services.Vehicle;

public class VehicleServices(IVehicleRepository vehicleRepository) : IVehicleServices
{
    private readonly IVehicleRepository _vehicleRepository = vehicleRepository;
    public async Task<IEnumerable<VehicleDto>> GetAll(int page, int size, string search)
    {
        var vehicleList = await _vehicleRepository.GetAll(page, size, search);
        return vehicleList.ConvertVehiclesToDtos();
    }
    public async Task<VehicleDto> GetByPlate(string plate)
    {
        var vehicle = await _vehicleRepository.GetByPlate(plate);
        return vehicle.ConvertVehicleToDto();
    }
    public async Task<VehicleDto> GetByRenavam(string renavam)
    {
        var vehicle = await _vehicleRepository.GetByRenavam(renavam);
        return vehicle.ConvertVehicleToDto();
    }
    public async Task Create(VehicleDto vehicle)
    {
        await _vehicleRepository.Create(vehicle.ConvertDtoToVehicle());
    }
    public async Task Update(VehicleDto vehicle)
    {
        await _vehicleRepository.Update(vehicle.ConvertDtoToVehicle());
    }
    public async Task Delete(string renavam)
    {
        await _vehicleRepository.Delete(renavam);
    }
    public async Task<int> TotalVehicle()
    {
        int totalVehicle = await _vehicleRepository.TotalVehicle();
        return totalVehicle;
    }
}
