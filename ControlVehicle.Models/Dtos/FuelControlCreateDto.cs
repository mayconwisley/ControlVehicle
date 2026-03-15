namespace ControlVehicle.Models.Dtos;

public sealed record FuelControlCreateDto(
    Guid VehicleId,
    Guid DriverId,
    decimal InitialKm,
    decimal Value,
    DateTime Date,
    decimal Liters,
    string? Description
);
