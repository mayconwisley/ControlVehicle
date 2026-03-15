namespace ControlVehicle.Models.Dtos;

public sealed record MaintenanceControlDto(
    Guid Id,
    Guid VehicleId,
    DateTime Date,
    decimal Value,
    string? Description
);
