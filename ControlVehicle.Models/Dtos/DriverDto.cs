using ControlVehicle.Domain.ValueObjects;

namespace ControlVehicle.Models.Dtos;

public sealed record DriverDto(
	Guid Id,
	string Name,
	Cnh Cnh,
	CategoryCnh CategoryCnh,
	DateOnly DateExpiration,
	bool Active
);
