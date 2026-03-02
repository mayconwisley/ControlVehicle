using ControlVehicle.Domain.Entities;
using ControlVehicle.Domain.Pagination;
using ControlVehicle.Domain.Repositories;
using ControlVehicle.Domain.ValueObjects;
using ControlVehicle.Infra.Database;
using Microsoft.EntityFrameworkCore;

namespace ControlVehicle.Infra.Repositories;

public sealed class DriverRepository(VehicleDbContext _db) : IDriverRepository
{
	public async Task Create(Driver driver, CancellationToken ct = default)
		=> await _db.Drivers.AddAsync(driver, ct);
	public void Delete(Driver driver)
		=> _db.Drivers.Remove(driver);
	public async Task<Driver?> GetByCnh(Cnh cnh, CancellationToken ct = default)
	=> await _db.Drivers
		.SingleOrDefaultAsync(s => s.Cnh.Number == cnh.Number, ct);
	public async Task<Driver?> GetById(Guid id, CancellationToken ct = default)
		=> await _db.Drivers
			.SingleOrDefaultAsync(s => s.Id == id, ct);
	public void Update(Driver driver)
		=> _db.Drivers.Update(driver);
	public async Task<PagedData<Driver>> GetAll(int page = 1, int size = 5, string? search = null, CancellationToken ct = default)
	{
		page = page < 1 ? 1 : page;
		size = size < 1 ? 5 : size;

		IQueryable<Driver> query = _db.Drivers.AsNoTracking();

		if (!string.IsNullOrWhiteSpace(search))
		{
			var pattern = $"%{search.Trim()}%";
			query = query.Where(w =>
				   EF.Functions.ILike(w.Cnh.Number, pattern) ||
				   EF.Functions.ILike(w.Name, pattern) ||
				   EF.Functions.ILike(w.CategoryCnh.Value, pattern)
			);
		}

		var total = await query.CountAsync(ct);
		var items = await query
		   .OrderBy(x => x.Name)
		   .Skip((page - 1) * size)
		   .Take(size)
		   .ToListAsync(ct);
		return new PagedData<Driver>(items, total);
	}
}
