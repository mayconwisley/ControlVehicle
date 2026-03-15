namespace ControlVehicle.Models.Dtos;

public sealed record DriverCreateDto(
    string Name,
    string Cnh,
    string CategoryCnh,
    DateOnly DateExpiration,
    bool Active
);
