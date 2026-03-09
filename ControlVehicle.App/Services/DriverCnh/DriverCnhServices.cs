using ControlVehicle.App.Services.DriverCnh.Interface;
using ControlVehicle.Domain.Entities;
using ControlVehicle.Domain.Repositories;
using ControlVehicle.Domain.ValueObjects;
using ControlVehicle.Models.Dtos;
using ControlVehicle.Models.MappingDto;

namespace ControlVehicle.App.Services.DriverCnh;

public class DriverCnhServices(
	IDriverCnhRepository driverCnhRepository,
	IDriverRepository driverRepository,
	IUnitOfWork unitOfWork) : IDriverCnhServices
{
	private readonly IDriverCnhRepository _driverCnhRepository = driverCnhRepository;
	private readonly IDriverRepository _driverRepository = driverRepository;
	private readonly IUnitOfWork _unitOfWork = unitOfWork;

	public async Task<IEnumerable<DriverCnhDto>> GetAll(int page, int size, string search)
	{
		var paged = await _driverCnhRepository.GetAll(page, size, search);
		return paged.Items.ConvertDriverCnhsToDtos();
	}

	public async Task<DriverCnhDto?> GetByCnh(string cnh)
	{
		var cnhVo = Cnh.Create(cnh);
		var entity = await _driverCnhRepository.GetByCnh(cnhVo);
		return entity?.ConvertDriverCnhToDto();
	}

	public async Task<DriverCnhDto?> GetById(Guid id)
	{
		var entity = await _driverCnhRepository.GetById(id);
		return entity?.ConvertDriverCnhToDto();
	}

	public async Task Create(DriverCnhDto cnhDto)
	{
		var driver = await _driverRepository.GetById(cnhDto.DriverId);
		if (driver is null)
		{
			throw new InvalidOperationException("Motorista nao encontrado para vincular a CNH.");
		}

		var existingDriverCnh = await _driverCnhRepository.GetByDriverId(cnhDto.DriverId);
		if (existingDriverCnh is not null)
		{
			throw new InvalidOperationException("Motorista ja possui CNH cadastrada.");
		}

		var existingCnh = await _driverCnhRepository.GetByCnh(cnhDto.Cnh);
		if (existingCnh is not null)
		{
			throw new InvalidOperationException("Numero de CNH ja cadastrado.");
		}

		var entity = new ControlVehicle.Domain.Entities.DriverCnh(cnhDto.DriverId, cnhDto.Cnh, cnhDto.DateExpiration, cnhDto.Status);
		await _driverCnhRepository.Create(entity);
		await _unitOfWork.CommitAsync();
	}

	public async Task Update(DriverCnhDto cnhDto)
	{
		var entity = await _driverCnhRepository.GetById(cnhDto.Id);
		if (entity is null)
		{
			return;
		}

		var driver = await _driverRepository.GetById(cnhDto.DriverId);
		if (driver is null)
		{
			throw new InvalidOperationException("Motorista nao encontrado para vincular a CNH.");
		}

		var duplicatedCnh = await _driverCnhRepository.GetByCnh(cnhDto.Cnh);
		if (duplicatedCnh is not null && duplicatedCnh.Id != cnhDto.Id)
		{
			throw new InvalidOperationException("Numero de CNH ja cadastrado.");
		}

		entity.Update(cnhDto.DriverId, cnhDto.Cnh, cnhDto.DateExpiration, cnhDto.Status);
		_driverCnhRepository.Update(entity);
		await _unitOfWork.CommitAsync();
	}

	public async Task Delete(string cnh)
	{
		var cnhVo = Cnh.Create(cnh);
		var entity = await _driverCnhRepository.GetByCnh(cnhVo);
		if (entity is null)
		{
			return;
		}

		_driverCnhRepository.Delete(entity);
		await _unitOfWork.CommitAsync();
	}

	public async Task<int> Total()
	{
		var paged = await _driverCnhRepository.GetAll(1, 1, string.Empty);
		return paged.TotalCount;
	}
}

