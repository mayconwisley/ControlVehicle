using ControlVehicle.App.Services.VehicleControl.Interface;
using ControlVehicle.Domain.Repositories;
using ControlVehicle.Models.Dtos;
using ControlVehicle.Models.MappingDto;

namespace ControlVehicle.App.Services.VehicleControl;

public class VehicleControlServices(IVehicleControlRepository controlRepository, IUnitOfWork unitOfWork) : IVehicleControlServices
{
	private readonly IVehicleControlRepository _controlRepository = controlRepository;
	private readonly IUnitOfWork _unitOfWork = unitOfWork;

	public async Task<IEnumerable<VehicleControlDto>> GetAll(int page, int size, string search)
	{
		var pagedControls = await _controlRepository.GetAll(page, size, search);
		return pagedControls.Items.ConvertVehicleControlsToDtos();
	}

	public async Task<VehicleControlDto?> GetById(Guid id)
	{
		var control = await _controlRepository.GetById(id);
		return control?.ConvertVehicleControlToDto();
	}

	public async Task<Guid> Create(VehicleControlDto control)
	{
		var controlEntity = control.ConvertDtoToVehicleControl();
		await _controlRepository.Create(controlEntity);
		await _unitOfWork.CommitAsync();
		return controlEntity.Id;
	}

	public async Task Update(VehicleControlDto control)
	{
		var controlEntity = await _controlRepository.GetById(control.Id);
		if (controlEntity is null)
		{
			return;
		}

		controlEntity.Update(
			control.VehicleId,
			control.DriverId,
			control.DepartureDate,
			control.ArrivalDate,
			control.InitialKm,
			control.FinalKm,
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

	public async Task<int> TotalVehicleControl()
	{
		var pagedControls = await _controlRepository.GetAll(1, 1, string.Empty);
		return pagedControls.TotalCount;
	}
}
