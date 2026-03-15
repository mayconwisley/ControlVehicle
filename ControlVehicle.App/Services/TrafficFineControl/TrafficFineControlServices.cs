using ControlVehicle.App.Services.TrafficFineControl.Interface;
using ControlVehicle.Domain.Repositories;
using ControlVehicle.Models.Dtos;
using ControlVehicle.Models.MappingDto;

namespace ControlVehicle.App.Services.TrafficFineControl;

public class TrafficFineControlServices(ITrafficFineControlRepository controlRepository, IUnitOfWork unitOfWork) : ITrafficFineControlServices
{
    private readonly ITrafficFineControlRepository _controlRepository = controlRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<IEnumerable<TrafficFineControlDto>> GetAll(int page, int size, string search)
    {
        var pagedControls = await _controlRepository.GetAll(page, size, search);
        return pagedControls.Items.ConvertTrafficFineControlsToDtos();
    }

    public async Task<TrafficFineControlDto?> GetById(Guid id)
    {
        var control = await _controlRepository.GetById(id);
        return control?.ConvertTrafficFineControlToDto();
    }

    public async Task<Guid> Create(TrafficFineControlDto control)
    {
        var controlEntity = control.ConvertDtoToTrafficFineControl();
        await _controlRepository.Create(controlEntity);
        await _unitOfWork.CommitAsync();
        return controlEntity.Id;
    }

    public async Task Update(TrafficFineControlDto control)
    {
        var controlEntity = await _controlRepository.GetById(control.Id);
        if (controlEntity is null)
        {
            return;
        }

        controlEntity.Update(
            control.VehicleId,
            control.DriverId,
            control.Points,
            control.Value,
            control.Date,
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

    public async Task<int> TotalTrafficFineControl()
    {
        var pagedControls = await _controlRepository.GetAll(1, 1, string.Empty);
        return pagedControls.TotalCount;
    }
}
