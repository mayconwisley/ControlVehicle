using ControlVehicle.Domain.Entities;
using ControlVehicle.Models.Dtos;

namespace ControlVehicle.Models.MappingDto;

public static class VehicleMapping
{
	public static IEnumerable<VehicleDto> ConvertVehiclesToDtos(this IEnumerable<Vehicle> vehicles)
	{
		return vehicles.Select(s => s.ConvertVehicleToDto());
	}
	public static IEnumerable<Vehicle> ConvertDtosToVehicles(this IEnumerable<VehicleDto> vehicleDtos)
	{
		return vehicleDtos.Select(s => s.ConvertDtoToVehicle());
	}
	public static VehicleDto ConvertVehicleToDto(this Vehicle vehicle)
	{
		return new VehicleDto
		(
			vehicle.Id,
			vehicle.Renavam,
			vehicle.Model,
			vehicle.LicensePlate,
			vehicle.Fuel,
			vehicle.Chassi,
			vehicle.VehicleColor,
			vehicle.Active
		);
	}
	public static Vehicle ConvertDtoToVehicle(this VehicleDto vehicleDto)
	{
		return new Vehicle
		(
			vehicleDto.LicensePlate,
			vehicleDto.Model,
			vehicleDto.Renavam,
			vehicleDto.Chassi,
			vehicleDto.Fuel,
			vehicleDto.VehicleColor
		);
	}
}
