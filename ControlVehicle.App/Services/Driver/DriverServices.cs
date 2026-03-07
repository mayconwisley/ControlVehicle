using ControlVehicle.App.Services.Driver.Interface;
using ControlVehicle.Domain.Repositories;
using ControlVehicle.Domain.ValueObjects;
using ControlVehicle.Models.Dtos;
using ControlVehicle.Models.MappingDto;

namespace ControlVehicle.App.Services.Driver;

public class DriverServices(IDriverRepository driverRepository, IUnitOfWork unitOfWork) : IDriverServices
{
	private readonly IDriverRepository _driverRepository = driverRepository;
	private readonly IUnitOfWork _unitOfWork = unitOfWork;

	public async Task<IEnumerable<DriverDto>> GetAll(int page, int size, string search)
	{
		var pagedDrivers = await _driverRepository.GetAll(page, size, search);
		return pagedDrivers.Items.ConvertDriversToDtos();
	}

	public async Task<DriverDto?> GetByCnh(string cnh)
	{
		var driverCnh = Cnh.Create(cnh);
		var driver = await _driverRepository.GetByCnh(driverCnh);
		return driver?.ConvertDriverToDto();
	}

	public async Task Create(DriverDto driver)
	{
		var driverEntity = new ControlVehicle.Domain.Entities.Driver(driver.Name, driver.Cnh, driver.CategoryCnh, driver.DateExpiration);
		await _driverRepository.Create(driverEntity);
		await _unitOfWork.CommitAsync();
	}

	public async Task Update(DriverDto driver)
	{
		var driverEntity = await _driverRepository.GetById(driver.Id);
		if (driverEntity is null)
		{
			return;
		}

		driverEntity.Update(driver.Name, driver.Cnh, driver.CategoryCnh, driver.DateExpiration);
		_driverRepository.Update(driverEntity);
		await _unitOfWork.CommitAsync();
	}

	public async Task Delete(string cnh)
	{
		var driverCnh = Cnh.Create(cnh);
		var driverEntity = await _driverRepository.GetByCnh(driverCnh);
		if (driverEntity is null)
		{
			return;
		}

		_driverRepository.Delete(driverEntity);
		await _unitOfWork.CommitAsync();
	}

	public async Task<int> TotalDriver()
	{
		var pagedDrivers = await _driverRepository.GetAll(1, 1, string.Empty);
		return pagedDrivers.TotalCount;
	}
}
