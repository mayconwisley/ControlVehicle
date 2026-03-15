namespace ControlVehicle.Models.Dtos;

public sealed record TrafficFineControlDto(
    Guid Id,
    Guid VehicleId,
    Guid DriverId,
    int Points,
    decimal Value,
    DateTime Date,
    string? Description
);
