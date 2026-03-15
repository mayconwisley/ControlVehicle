namespace ControlVehicle.Models.Dtos;

public sealed record MaintenanceControlUpdateDto(
    Guid Id,
    Guid VehicleId,
    DateTime Date,
    decimal Value,
    string? Description
);
