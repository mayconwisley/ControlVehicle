using ControlVehicle.Domain.Entities;
using ControlVehicle.Domain.ValueObjects;

namespace ControlVehicle.Domain.Repositories;

public interface IVehicleRepository
{
	public Task<IReadOnlyList<Vehicle>> GetAll(int page, int size, string search, CancellationToken ct = default);
	public Task<Vehicle> GetById(Guid id, CancellationToken ct = default);
	public Task<Vehicle> GetByLicensePlate(LicensePlate licensePlate, CancellationToken ct = default);
	public Task<Vehicle> GetByRenavam(Renavam renavam, CancellationToken ct = default);
	public Task Create(Vehicle vehicle, CancellationToken ct = default);
	public Task Update(Vehicle vehicle, CancellationToken ct = default);
	public Task Delete(Vehicle vehicle, CancellationToken ct = default);
}
