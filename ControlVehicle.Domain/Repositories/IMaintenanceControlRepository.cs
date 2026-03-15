using ControlVehicle.Domain.Entities;
using ControlVehicle.Domain.Pagination;

namespace ControlVehicle.Domain.Repositories;

public interface IMaintenanceControlRepository
{
    Task<PagedData<MaintenanceControl>> GetAll(int page = 1, int size = 5, string? search = null, CancellationToken ct = default);
    Task<MaintenanceControl?> GetById(Guid id, CancellationToken ct = default);
    Task Create(MaintenanceControl control, CancellationToken ct = default);
    void Update(MaintenanceControl control);
    void Delete(MaintenanceControl control);
}
