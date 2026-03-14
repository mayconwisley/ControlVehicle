using ControlVehicle.Domain.Enums;

namespace ControlVehicle.Models.Dtos;

public sealed record VehicleDto(
    Guid Id,
    string Renavam,
    string Model,
    string LicensePlate,
    FuelEnum Fuel,
    string? Chassi,
    VehicleColorEnum VehicleColor,
    bool Active
);
