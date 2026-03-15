namespace ControlVehicle.Models.Dtos;

public sealed record DriverUpdateDto(
    Guid Id,
    string Name,
    string Cnh,
    string CategoryCnh,
    DateOnly DateExpiration,
    bool Active
);
