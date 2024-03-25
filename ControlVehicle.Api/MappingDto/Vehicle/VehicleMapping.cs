using ControlVehicle.Api.Models.Vehicle;
using ControlVehicle.Dto.Vehicle;

namespace ControlVehicle.Api.MappingDto.Vehicle;

public static class VehicleMapping
{
    public static IEnumerable<VehicleDto> ConvertVehiclesToDtos(this IEnumerable<VehicleModel> vehicles)
    {
        return (from vehicle in vehicles
                select new VehicleDto
                {
                    Id = vehicle.Id,
                    Renavam = vehicle.Renavam,
                    Model = vehicle.Model,
                    Plate = vehicle.Plate,
                    Fuel = vehicle.Fuel,
                    Chassi = vehicle.Chassi,
                    Active = vehicle.Active,
                    Color = vehicle.Color

                }).ToList();
    }
    public static IEnumerable<VehicleModel> ConvertDtosToVehicles(this IEnumerable<VehicleDto> vehicles)
    {
        return (from vehicle in vehicles
                select new VehicleModel
                {
                    Id = vehicle.Id,
                    Renavam = vehicle.Renavam,
                    Model = vehicle.Model,
                    Plate = vehicle.Plate,
                    Fuel = vehicle.Fuel,
                    Chassi = vehicle.Chassi,
                    Active = vehicle.Active,
                    Color = vehicle.Color

                }).ToList();
    }
    public static VehicleDto ConvertVehicleToDto(this VehicleModel vehicle)
    {
        return new VehicleDto
        {
            Id = vehicle.Id,
            Renavam = vehicle.Renavam,
            Model = vehicle.Model,
            Plate = vehicle.Plate,
            Fuel = vehicle.Fuel,
            Chassi = vehicle.Chassi,
            Active = vehicle.Active,
            Color = vehicle.Color

        };
    }
    public static VehicleModel ConvertDtoToVehicle(this VehicleDto vehicle)
    {
        return new VehicleModel
        {
            Id = vehicle.Id,
            Renavam = vehicle.Renavam,
            Model = vehicle.Model,
            Plate = vehicle.Plate,
            Fuel = vehicle.Fuel,
            Chassi = vehicle.Chassi,
            Active = vehicle.Active,
            Color = vehicle.Color

        };
    }
}
