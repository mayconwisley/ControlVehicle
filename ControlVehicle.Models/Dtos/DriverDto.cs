namespace ControlVehicle.Models.Dtos;

public sealed record DriverDto(
    Guid Id,
    string Name,
    string Cnh,
    string CategoryCnh,
    DateOnly DateExpiration,
    bool Active
);
