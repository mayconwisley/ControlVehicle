using ControlVehicle.Domain.Entities;
using ControlVehicle.Models.Dtos;

namespace ControlVehicle.Models.MappingDto;

public static class DriverCnhMapping
{
	public static IEnumerable<DriverCnhDto> ConvertDriverCnhsToDtos(this IEnumerable<DriverCnh> entities)
		=> entities.Select(ConvertDriverCnhToDto);

	public static DriverCnhDto ConvertDriverCnhToDto(this DriverCnh entity)
		=> new(
			entity.Id,
			entity.DriverId,
			entity.Cnh,
			entity.DateExpiration,
			entity.Status);
}
