namespace ControlVehicle.Models.Dtos;

public sealed record TrafficFineControlUpdateDto(
    Guid Id,
    Guid VehicleId,
    Guid DriverId,
    int Points,
    decimal Value,
    DateTime Date,
    string? Description
);
