using ControlVehicle.Domain.Entities;
using ControlVehicle.Domain.Pagination;
using ControlVehicle.Domain.ValueObjects;

namespace ControlVehicle.Domain.Repositories;

public interface IDriverRepository
{
	public Task<PagedData<Driver>> GetAll(int page = 1, int size = 5, string? search = null, CancellationToken ct = default);
	public Task<Driver?> GetById(Guid id, CancellationToken ct = default);
	public Task<Driver?> GetByCnh(Cnh cnh, CancellationToken ct = default);
	public Task Create(Driver driver, CancellationToken ct = default);
	public void Update(Driver driver);
	public void Delete(Driver driver);
}
