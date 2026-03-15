using ControlVehicle.Domain.Entities;
using ControlVehicle.Domain.ValueObjects;
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
            vehicle.Renavam.Value,
            vehicle.Model,
            vehicle.LicensePlate.Value,
            vehicle.Fuel,
            vehicle.Chassi?.Value,
            vehicle.VehicleColor,
            vehicle.Active
        );
    }
    public static Vehicle ConvertDtoToVehicle(this VehicleDto vehicleDto)
    {
        return new Vehicle
        (
            LicensePlate.Create(vehicleDto.LicensePlate),
            vehicleDto.Model,
            Renavam.Create(vehicleDto.Renavam),
            vehicleDto.Chassi is null ? null : Chassi.Create(vehicleDto.Chassi),
            vehicleDto.Fuel,
            vehicleDto.VehicleColor
        );
    }
}
