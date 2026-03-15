namespace ControlVehicle.Models.Dtos;

public sealed record VehicleControlCreateDto(
    Guid VehicleId,
    Guid DriverId,
    DateTime DepartureDate,
    DateTime ArrivalDate,
    decimal InitialKm,
    decimal FinalKm,
    string Description
);
