using ControlVehicle.Models.Dtos;

namespace ControlVehicle.App.Services.MaintenanceControl.Interface;

public interface IMaintenanceControlServices
{
    Task<IEnumerable<MaintenanceControlDto>> GetAll(int page, int size, string search);
    Task<MaintenanceControlDto?> GetById(Guid id);
    Task<MaintenanceControlDto> Create(MaintenanceControlCreateDto control);
    Task<MaintenanceControlDto?> Update(MaintenanceControlUpdateDto control);
    Task Delete(Guid id);
    Task<int> TotalMaintenanceControl();
}
