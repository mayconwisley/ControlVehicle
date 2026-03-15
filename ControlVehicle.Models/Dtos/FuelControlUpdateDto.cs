namespace ControlVehicle.Models.Dtos;

public sealed record FuelControlUpdateDto(
    Guid Id,
    Guid VehicleId,
    Guid DriverId,
    decimal InitialKm,
    decimal Value,
    DateTime Date,
    decimal Liters,
    string? Description
);
