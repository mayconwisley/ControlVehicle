using ControlVehicle.Domain.Entities;
using ControlVehicle.Domain.Pagination;

namespace ControlVehicle.Domain.Repositories;

public interface IVehicleControlRepository
{
	Task<PagedData<VehicleControl>> GetAll(int page = 1, int size = 5, string? search = null, CancellationToken ct = default);
	Task<VehicleControl?> GetById(Guid id, CancellationToken ct = default);
	Task Create(VehicleControl control, CancellationToken ct = default);
	void Update(VehicleControl control);
	void Delete(VehicleControl control);
}
