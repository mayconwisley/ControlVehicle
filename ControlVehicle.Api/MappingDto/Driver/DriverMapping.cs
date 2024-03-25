using ControlVehicle.Api.Models.Driver;
using ControlVehicle.Dto.Driver;

namespace ControlVehicle.Api.MappingDto.Driver;

public static class DriverMapping
{
    public static IEnumerable<DriverDto> ConvertDriversToDtos(this IEnumerable<DriverModel> drivers)
    {
        return (from driver in drivers
                select new DriverDto
                {
                    Id = driver.Id,
                    Name = driver.Name,
                    CNH = driver.CNH,
                    Active = driver.Active,
                    CategoryCNH = driver.CategoryCNH,
                    DateExpiration = driver.DateExpiration

                }).ToList();
    }
    public static IEnumerable<DriverModel> ConvertDtosToDrivers(this IEnumerable<DriverDto> drivers)
    {
        return (from driver in drivers
                select new DriverModel
                {
                    Id = driver.Id,
                    Name = driver.Name,
                    CNH = driver.CNH,
                    Active = driver.Active,
                    CategoryCNH = driver.CategoryCNH,
                    DateExpiration = driver.DateExpiration

                }).ToList();
    }
    public static DriverDto ConvertDriverToDto(this DriverModel driver)
    {
        return new DriverDto
        {
            Id = driver.Id,
            Name = driver.Name,
            CNH = driver.CNH,
            Active = driver.Active,
            CategoryCNH = driver.CategoryCNH,
            DateExpiration = driver.DateExpiration

        };
    }
    public static DriverModel ConvertDtoToDriver(this DriverDto driver)
    {
        return new DriverModel
        {
            Id = driver.Id,
            Name = driver.Name,
            CNH = driver.CNH,
            Active = driver.Active,
            CategoryCNH = driver.CategoryCNH,
            DateExpiration = driver.DateExpiration

        };
    }
}
