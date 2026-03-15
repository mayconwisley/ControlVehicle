using ControlVehicle.Domain.Entities;
using ControlVehicle.Models.Dtos;

namespace ControlVehicle.Models.MappingDto;

public static class FuelControlMapping
{
    public static IEnumerable<FuelControlDto> ConvertFuelControlsToDtos(this IEnumerable<FuelControl> controls)
    {
        return controls.Select(s => s.ConvertFuelControlToDto());
    }

    public static IEnumerable<FuelControl> ConvertDtosToFuelControls(this IEnumerable<FuelControlDto> controlDtos)
    {
        return controlDtos.Select(s => s.ConvertDtoToFuelControl());
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

    public static FuelControl ConvertDtoToFuelControl(this FuelControlDto controlDto)
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
