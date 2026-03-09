using ControlVehicle.Domain.Enums;
using ControlVehicle.Domain.ValueObjects;

namespace ControlVehicle.Models.Dtos;

public sealed record DriverCnhDto(
	Guid Id,
	Guid DriverId,
	Cnh Cnh,
	DateOnly DateExpiration,
	EmploymentStatusEnum Status
);
