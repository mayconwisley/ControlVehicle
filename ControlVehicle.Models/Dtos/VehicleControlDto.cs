namespace ControlVehicle.Models.Dtos;

public sealed record VehicleControlDto(
    Guid Id,
    Guid VehicleId,
    Guid DriverId,
    DateTime DepartureDate,
    DateTime ArrivalDate,
    decimal InitialKm,
    decimal FinalKm,
    string Description
);
