using ControlVehicle.App.Services.MaintenanceControl.Interface;
using ControlVehicle.Domain.Repositories;
using ControlVehicle.Models.Dtos;
using ControlVehicle.Models.MappingDto;

namespace ControlVehicle.App.Services.MaintenanceControl;

public class MaintenanceControlServices(IMaintenanceControlRepository controlRepository, IUnitOfWork unitOfWork) : IMaintenanceControlServices
{
    private readonly IMaintenanceControlRepository _controlRepository = controlRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<IEnumerable<MaintenanceControlDto>> GetAll(int page, int size, string search)
    {
        var pagedControls = await _controlRepository.GetAll(page, size, search);
        return pagedControls.Items.ConvertMaintenanceControlsToDtos();
    }

    public async Task<MaintenanceControlDto?> GetById(Guid id)
    {
        var control = await _controlRepository.GetById(id);
        return control?.ConvertMaintenanceControlToDto();
    }

    public async Task<Guid> Create(MaintenanceControlDto control)
    {
        var controlEntity = control.ConvertDtoToMaintenanceControl();
        await _controlRepository.Create(controlEntity);
        await _unitOfWork.CommitAsync();
        return controlEntity.Id;
    }

    public async Task Update(MaintenanceControlDto control)
    {
        var controlEntity = await _controlRepository.GetById(control.Id);
        if (controlEntity is null)
        {
            return;
        }

        controlEntity.Update(
            control.VehicleId,
            control.Date,
            control.Value,
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

    public async Task<int> TotalMaintenanceControl()
    {
        var pagedControls = await _controlRepository.GetAll(1, 1, string.Empty);
        return pagedControls.TotalCount;
    }
}
