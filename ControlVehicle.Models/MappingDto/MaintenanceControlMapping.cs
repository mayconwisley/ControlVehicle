using ControlVehicle.Domain.Entities;
using ControlVehicle.Models.Dtos;

namespace ControlVehicle.Models.MappingDto;

public static class MaintenanceControlMapping
{
    public static IEnumerable<MaintenanceControlDto> ConvertMaintenanceControlsToDtos(this IEnumerable<MaintenanceControl> controls)
    {
        return controls.Select(s => s.ConvertMaintenanceControlToDto());
    }

    public static IEnumerable<MaintenanceControl> ConvertDtosToMaintenanceControls(this IEnumerable<MaintenanceControlDto> controlDtos)
    {
        return controlDtos.Select(s => s.ConvertDtoToMaintenanceControl());
    }

    public static MaintenanceControlDto ConvertMaintenanceControlToDto(this MaintenanceControl control)
    {
        return new MaintenanceControlDto
        (
            control.Id,
            control.VehicleId,
            control.Date,
            control.Value,
            control.Description
        );
    }

    public static MaintenanceControl ConvertDtoToMaintenanceControl(this MaintenanceControlDto controlDto)
    {
        return new MaintenanceControl
        (
            controlDto.VehicleId,
            controlDto.Date,
            controlDto.Value,
            controlDto.Description
        );
    }
}
