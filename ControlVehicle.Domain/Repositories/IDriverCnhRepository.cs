using ControlVehicle.Domain.Entities;
using ControlVehicle.Domain.Pagination;
using ControlVehicle.Domain.ValueObjects;

namespace ControlVehicle.Domain.Repositories;

public interface IDriverCnhRepository
{
	Task<PagedData<DriverCnh>> GetAll(int page, int size, string? search = null, CancellationToken ct = default);
	Task<DriverCnh?> GetById(Guid id, CancellationToken ct = default);
	Task<DriverCnh?> GetByCnh(Cnh cnh, CancellationToken ct = default);
	Task<DriverCnh?> GetByDriverId(Guid driverId, CancellationToken ct = default);
	Task Create(DriverCnh driverCnh, CancellationToken ct = default);
	void Update(DriverCnh driverCnh);
	void Delete(DriverCnh driverCnh);
}
