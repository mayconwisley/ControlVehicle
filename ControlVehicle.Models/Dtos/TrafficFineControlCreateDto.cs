namespace ControlVehicle.Models.Dtos;

public sealed record TrafficFineControlCreateDto(
    Guid VehicleId,
    Guid DriverId,
    int Points,
    decimal Value,
    DateTime Date,
    string? Description
);
