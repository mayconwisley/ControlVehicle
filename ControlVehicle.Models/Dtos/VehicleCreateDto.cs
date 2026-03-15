using ControlVehicle.Domain.Enums;

namespace ControlVehicle.Models.Dtos;

public sealed record VehicleCreateDto(
    string Renavam,
    string Model,
    string LicensePlate,
    FuelEnum Fuel,
    string? Chassi,
    VehicleColorEnum VehicleColor,
    bool Active
);
