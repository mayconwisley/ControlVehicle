using ControlVehicle.Domain.Enums;
using ControlVehicle.Domain.ValueObjects;

namespace ControlVehicle.Models.Dtos;

public sealed record VehicleDto(
	Guid Id,
	Renavam Renavam,
	string Model,
	LicensePlate LicensePlate,
	FuelEnum Fuel,
	Chassi? Chassi,
	VehicleColorEnum VehicleColor,
	bool Active
);