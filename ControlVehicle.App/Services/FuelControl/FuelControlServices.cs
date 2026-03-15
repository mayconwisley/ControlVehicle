using ControlVehicle.App.Services.FuelControl.Interface;
using ControlVehicle.Domain.Repositories;
using ControlVehicle.Models.Dtos;
using ControlVehicle.Models.MappingDto;

namespace ControlVehicle.App.Services.FuelControl;

public class FuelControlServices(IFuelControlRepository controlRepository, IUnitOfWork unitOfWork) : IFuelControlServices
{
	private readonly IFuelControlRepository _controlRepository = controlRepository;
	private readonly IUnitOfWork _unitOfWork = unitOfWork;

	public async Task<IEnumerable<FuelControlDto>> GetAll(int page, int size, string search)
	{
		var pagedControls = await _controlRepository.GetAll(page, size, search);
		return pagedControls.Items.ConvertFuelControlsToDtos();
	}

	public async Task<FuelControlDto?> GetById(Guid id)
	{
		var control = await _controlRepository.GetById(id);
		return control?.ConvertFuelControlToDto();
	}

	public async Task<Guid> Create(FuelControlDto control)
	{
		var controlEntity = control.ConvertDtoToFuelControl();
		await _controlRepository.Create(controlEntity);
		await _unitOfWork.CommitAsync();
		return controlEntity.Id;
	}

	public async Task Update(FuelControlDto control)
	{
		var controlEntity = await _controlRepository.GetById(control.Id);
		if (controlEntity is null)
		{
			return;
		}

		controlEntity.Update(
			control.VehicleId,
			control.DriverId,
			control.InitialKm,
			control.Value,
			control.Date,
			control.Liters,
			control.Description);

		_controlRepository.Update(controlEntity);
		await _unitOfWork.CommitAsync();
	}

	public async Task Delete(Guid id)
	{
		var controlEntity = await _controlRepository.GetById(id);
		if (controlEntity is null)
		{
			return;
		}

		_controlRepository.Delete(controlEntity);
		await _unitOfWork.CommitAsync();
	}

	public async Task<int> TotalFuelControl()
	{
		var pagedControls = await _controlRepository.GetAll(1, 1, string.Empty);
		return pagedControls.TotalCount;
	}
}
