using ControlVehicle.Domain.Entities;
using ControlVehicle.Models.Dtos;

namespace ControlVehicle.Models.MappingDto;

public static class TrafficFineControlMapping
{
    public static IEnumerable<TrafficFineControlDto> ConvertTrafficFineControlsToDtos(this IEnumerable<TrafficFineControl> controls)
    {
        return controls.Select(s => s.ConvertTrafficFineControlToDto());
    }

    public static IEnumerable<TrafficFineControl> ConvertDtosToTrafficFineControls(this IEnumerable<TrafficFineControlDto> controlDtos)
    {
        return controlDtos.Select(s => s.ConvertDtoToTrafficFineControl());
    }

    public static TrafficFineControlDto ConvertTrafficFineControlToDto(this TrafficFineControl control)
    {
        return new TrafficFineControlDto
        (
            control.Id,
            control.VehicleId,
            control.DriverId,
            control.Points,
            control.Value,
            control.Date,
            control.Description
        );
    }

    public static TrafficFineControl ConvertDtoToTrafficFineControl(this TrafficFineControlDto controlDto)
    {
        return new TrafficFineControl
        (
            controlDto.VehicleId,
            controlDto.DriverId,
            controlDto.Points,
            controlDto.Value,
            controlDto.Date,
            controlDto.Description
        );
    }
}
