using ControlVehicle.App.Services.Vehicle.Interface;
using ControlVehicle.Domain.Repositories;
using ControlVehicle.Domain.ValueObjects;
using ControlVehicle.Models.Dtos;
using ControlVehicle.Models.MappingDto;

namespace ControlVehicle.App.Services.Vehicle;

public class VehicleServices(IVehicleRepository vehicleRepository, IUnitOfWork unitOfWork) : IVehicleServices
{
	private readonly IVehicleRepository _vehicleRepository = vehicleRepository;
	private readonly IUnitOfWork _unitOfWork = unitOfWork;

	public async Task<IEnumerable<VehicleDto>> GetAll(int page, int size, string search)
	{
		var pagedVehicles = await _vehicleRepository.GetAll(page, size, search);
		return pagedVehicles.Items.ConvertVehiclesToDtos();
	}

	public async Task<VehicleDto?> GetByPlate(string plate)
	{
		var licensePlate = LicensePlate.Create(plate);
		var vehicle = await _vehicleRepository.GetByLicensePlate(licensePlate);
		return vehicle?.ConvertVehicleToDto();
	}

	public async Task<VehicleDto?> GetByRenavam(string renavam)
	{
		var vehicleRenavam = Renavam.Create(renavam);
		var vehicle = await _vehicleRepository.GetByRenavam(vehicleRenavam);
		return vehicle?.ConvertVehicleToDto();
	}

	public async Task Create(VehicleDto vehicle)
	{
		var vehicleEntity = new ControlVehicle.Domain.Entities.Vehicle(
			vehicle.LicensePlate,
			vehicle.Model,
			vehicle.Renavam,
			vehicle.Chassi,
			vehicle.Fuel,
			vehicle.VehicleColor);

		await _vehicleRepository.Create(vehicleEntity);
		await _unitOfWork.CommitAsync();
	}

	public async Task Update(VehicleDto vehicle)
	{
		var vehicleEntity = await _vehicleRepository.GetById(vehicle.Id);
		if (vehicleEntity is null)
		{
			return;
		}

		vehicleEntity.Update(
			vehicle.LicensePlate,
			vehicle.Model,
			vehicle.Renavam,
			vehicle.Chassi,
			vehicle.Fuel,
			vehicle.VehicleColor);

		_vehicleRepository.Update(vehicleEntity);
		await _unitOfWork.CommitAsync();
	}

	public async Task Delete(string renavam)
	{
		var vehicleRenavam = Renavam.Create(renavam);
		var vehicleEntity = await _vehicleRepository.GetByRenavam(vehicleRenavam);
		if (vehicleEntity is null)
		{
			return;
		}

		_vehicleRepository.Delete(vehicleEntity);
		await _unitOfWork.CommitAsync();
	}

	public async Task<int> TotalVehicle()
	{
		var pagedVehicles = await _vehicleRepository.GetAll(1, 1, string.Empty);
		return pagedVehicles.TotalCount;
	}
}
