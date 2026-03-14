using ControlVehicle.Domain.Entities;
using ControlVehicle.Domain.Pagination;
using ControlVehicle.Domain.ValueObjects;

namespace ControlVehicle.Domain.Repositories;

public interface IVehicleRepository
{
	public Task<PagedData<Vehicle>> GetAll(int page = 1, int size = 5, string? search = null, CancellationToken ct = default);
	public Task<Vehicle?> GetById(Guid id, CancellationToken ct = default);
	public Task<Vehicle?> GetByLicensePlate(LicensePlate licensePlate, CancellationToken ct = default);
	public Task<Vehicle?> GetByRenavam(Renavam renavam, CancellationToken ct = default);
	public Task Create(Vehicle vehicle, CancellationToken ct = default);
	public void Update(Vehicle vehicle);
	public void Delete(Vehicle vehicle);
}
