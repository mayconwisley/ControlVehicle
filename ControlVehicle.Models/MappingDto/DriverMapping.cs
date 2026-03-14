using ControlVehicle.Domain.Entities;
using ControlVehicle.Domain.ValueObjects;
using ControlVehicle.Models.Dtos;

namespace ControlVehicle.Models.MappingDto;

public static class DriverMapping
{
	public static IEnumerable<DriverDto> ConvertDriversToDtos(this IEnumerable<Driver> drivers)
	{
		return drivers.Select(s => s.ConvertDriverToDto());
	}
	public static IEnumerable<Driver> ConvertDtosToDrivers(this IEnumerable<DriverDto> driversDto)
	{
		return driversDto.Select(s => s.ConvertDtoToDriver());
	}
	public static DriverDto ConvertDriverToDto(this Driver driver)
	{
		return new DriverDto
		(
			driver.Id,
			driver.Name,
			driver.Cnh.Number,
			driver.CategoryCnh.Value,
			driver.DateExpiration,
			driver.Active
		);
	}
	public static Driver ConvertDtoToDriver(this DriverDto driverDto)
	{
		return new Driver
		(
			driverDto.Name,
			Cnh.Create(driverDto.Cnh),
			CategoryCnh.Create(driverDto.CategoryCnh),
			driverDto.DateExpiration
		);
	}
}
