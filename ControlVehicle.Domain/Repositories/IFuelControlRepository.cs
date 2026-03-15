using ControlVehicle.Domain.Entities;
using ControlVehicle.Domain.Pagination;

namespace ControlVehicle.Domain.Repositories;

public interface IFuelControlRepository
{
	Task<PagedData<FuelControl>> GetAll(int page = 1, int size = 5, string? search = null, CancellationToken ct = default);
	Task<FuelControl?> GetById(Guid id, CancellationToken ct = default);
	Task Create(FuelControl control, CancellationToken ct = default);
	void Update(FuelControl control);
	void Delete(FuelControl control);
}
