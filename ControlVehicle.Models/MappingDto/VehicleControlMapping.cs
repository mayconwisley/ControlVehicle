using ControlVehicle.Domain.Entities;
using ControlVehicle.Models.Dtos;

namespace ControlVehicle.Models.MappingDto;

public static class VehicleControlMapping
{
    public static IEnumerable<VehicleControlDto> ConvertVehicleControlsToDtos(this IEnumerable<VehicleControl> controls)
    {
        return controls.Select(s => s.ConvertVehicleControlToDto());
    }

    public static IEnumerable<VehicleControl> ConvertDtosToVehicleControls(this IEnumerable<VehicleControlDto> controlDtos)
    {
        return controlDtos.Select(s => s.ConvertDtoToVehicleControl());
    }

    public static VehicleControlDto ConvertVehicleControlToDto(this VehicleControl control)
    {
        return new VehicleControlDto
        (
            control.Id,
            control.VehicleId,
            control.DriverId,
            control.DepartureDate,
            control.ArrivalDate,
            control.InitialKm,
            control.FinalKm,
            control.Description
        );
    }

    public static VehicleControl ConvertDtoToVehicleControl(this VehicleControlDto controlDto)
    {
        return new VehicleControl
        (
            controlDto.VehicleId,
            controlDto.DriverId,
            controlDto.DepartureDate,
            controlDto.ArrivalDate,
            controlDto.InitialKm,
            controlDto.FinalKm,
            controlDto.Description
        );
    }
}
