using ControlVehicle.Api.MappingDto.Driver;
using ControlVehicle.Api.Repository.Driver.Interface;
using ControlVehicle.Api.Services.Driver.Interface;
using ControlVehicle.Dto.Driver;

namespace ControlVehicle.Api.Services.Driver;

public class DriverServices(IDriverRepository driverRepository) : IDriverServices
{
    private readonly IDriverRepository _driverRepository = driverRepository;

    public async Task<IEnumerable<DriverDto>> GetAll(int page, int size, string search)
    {
        var driverList = await _driverRepository.GetAll(page, size, search);
        return driverList.ConvertDriversToDtos();
    }

    public async Task<DriverDto> GetByCnh(string cnh)
    {
        var driver = await _driverRepository.GetByCnh(cnh);
        return driver.ConvertDriverToDto();
    }
    public async Task Create(DriverDto driver)
    {
        await _driverRepository.Create(driver.ConvertDtoToDriver());
    }
    public async Task Update(DriverDto driver)
    {
        await _driverRepository.Update(driver.ConvertDtoToDriver());
    }
    public async Task Delete(string cnh)
    {
        await _driverRepository.Delete(cnh);
    }

    public async Task<int> TotalDriver()
    {
        int totalDriver = await _driverRepository.TotalDriver();
        return totalDriver;
    }


}
