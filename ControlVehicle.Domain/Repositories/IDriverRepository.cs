using ControlVehicle.Domain.Entities;
using ControlVehicle.Domain.ValueObjects;

namespace ControlVehicle.Domain.Repositories;

public interface IDriverRepository
{
	public Task<IReadOnlyList<Driver>> GetAll(int page = 1, int size = 5, string? search = "", CancellationToken ct = default);
	public Task<Driver?> GetById(Guid id, CancellationToken ct = default);
	public Task<Driver?> GetByCnh(Cnh cnh, CancellationToken ct = default);
	public Task Create(Driver driver, CancellationToken ct = default);
	public void Update(Driver driver, CancellationToken ct = default);
	public void Delete(Driver driver, CancellationToken ct = default);
}
