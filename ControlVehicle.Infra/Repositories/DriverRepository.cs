using ControlVehicle.Domain.Entities;
using ControlVehicle.Domain.Repositories;
using ControlVehicle.Domain.ValueObjects;
using ControlVehicle.Infra.Database;
using Microsoft.EntityFrameworkCore;

namespace ControlVehicle.Infra.Repositories;

public sealed class DriverRepository(VehicleDbContext _db) : IDriverRepository
{
	public async Task Create(Driver driver, CancellationToken ct = default)
		=> await _db.Drivers.AddAsync(driver, ct);


	public void Delete(Driver driver, CancellationToken ct = default)
	{
		_db.Drivers.Remove(driver);
	}

	public async Task<IReadOnlyList<Driver>> GetAll(int page = 1, int size = 5, string? search = "", CancellationToken ct = default)
	{
		page = page < 1 ? 1 : page;
		size = size < 1 ? 5 : size;

		var query = _db.Drivers
			.AsNoTracking()
			.OrderBy(x => x.Name)
			.AsQueryable();


		if (!string.IsNullOrWhiteSpace(search))
		{
			var pattern = $"%{search.Trim()}%";
			query = query.Where(w =>
				   EF.Functions.ILike(w.Cnh.Number, pattern) ||
				   EF.Functions.ILike(w.Name, pattern) ||
				   EF.Functions.ILike(w.CategoryCnh.Value, pattern)
			);
		}

		return await query
		   .Skip((page - 1) * size)
		   .Take(size)
		   .ToListAsync(ct);
	}

	public async Task<Driver?> GetByCnh(Cnh cnh, CancellationToken ct = default)
	{
		return await _db.Drivers
			.SingleOrDefaultAsync(s => s.Cnh.Number == cnh.Number, ct);
	}

	public async Task<Driver?> GetById(Guid id, CancellationToken ct = default)
	{
		return await _db.Drivers
			.SingleOrDefaultAsync(s => s.Id == id, ct);
	}

	public void Update(Driver driver, CancellationToken ct = default)
	{
		_db.Drivers.Update(driver);
	}
}
