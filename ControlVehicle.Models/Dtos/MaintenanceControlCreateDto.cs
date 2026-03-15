namespace ControlVehicle.Models.Dtos;

public sealed record MaintenanceControlCreateDto(
    Guid VehicleId,
    DateTime Date,
    decimal Value,
    string? Description
);
