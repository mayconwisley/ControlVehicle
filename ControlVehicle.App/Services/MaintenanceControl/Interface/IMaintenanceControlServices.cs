using ControlVehicle.Models.Dtos;

namespace ControlVehicle.App.Services.MaintenanceControl.Interface;

public interface IMaintenanceControlServices
{
    Task<IEnumerable<MaintenanceControlDto>> GetAll(int page, int size, string search);
    Task<MaintenanceControlDto?> GetById(Guid id);
    Task<Guid> Create(MaintenanceControlDto control);
    Task Update(MaintenanceControlDto control);
    Task Delete(Guid id);
    Task<int> TotalMaintenanceControl();
}
