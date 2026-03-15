using ControlVehicle.Domain.Entities;
using ControlVehicle.Models.Dtos;

namespace ControlVehicle.Models.MappingDto;

public static class FuelControlMapping
{
    public static IEnumerable<FuelControlDto> ConvertFuelControlsToDtos(this IEnumerable<FuelControl> controls)
    {
        return controls.Select(s => s.ConvertFuelControlToDto());
    }

    public static IEnumerable<FuelControl> ConvertDtosToFuelControls(this IEnumerable<FuelControlCreateDto> controlDtos)
    {
        return controlDtos.Select(s => s.ConvertCreateDtoToFuelControl());
    }

    public static FuelControlDto ConvertFuelControlToDto(this FuelControl control)
    {
        return new FuelControlDto
        (
            control.Id,
            control.VehicleId,
            control.DriverId,
            control.InitialKm,
            control.Value,
            control.Date,
            control.Liters,
            control.Description
        );
    }

    public static FuelControl ConvertCreateDtoToFuelControl(this FuelControlCreateDto controlDto)
    {
        return new FuelControl
        (
            controlDto.VehicleId,
            controlDto.DriverId,
            controlDto.InitialKm,
            controlDto.Value,
            controlDto.Date,
            controlDto.Liters,
            controlDto.Description
        );
    }
}
