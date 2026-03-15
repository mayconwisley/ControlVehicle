using ControlVehicle.Domain.Entities;
using ControlVehicle.Domain.Pagination;
using ControlVehicle.Domain.Repositories;
using ControlVehicle.Infra.Database;
using Microsoft.EntityFrameworkCore;

namespace ControlVehicle.Infra.Repositories;

public sealed class FuelControlRepository(VehicleDbContext db) : IFuelControlRepository
{
	private readonly VehicleDbContext _db = db;

	public async Task<PagedData<FuelControl>> GetAll(int page = 1, int size = 5, string? search = null, CancellationToken ct = default)
	{
		page = page < 1 ? 1 : page;
		size = size < 1 ? 5 : size;

		IQueryable<FuelControl> query = _db.FuelControls.AsNoTracking();

		if (!string.IsNullOrWhiteSpace(search))
		{
			var trimmed = search.Trim();
			var pattern = $"%{trimmed}%";
			var hasId = Guid.TryParse(trimmed, out var id);

			query = query.Where(x =>
				(x.Description != null && EF.Functions.ILike(x.Description, pattern)) ||
				(hasId && (x.VehicleId == id || x.DriverId == id))
			);
		}

		var total = await query.CountAsync(ct);
		var items = await query
			.OrderByDescending(x => x.Date)
			.Skip((page - 1) * size)
			.Take(size)
			.ToListAsync(ct);

		return new PagedData<FuelControl>(items, total);
	}

	public async Task<FuelControl?> GetById(Guid id, CancellationToken ct = default)
		=> await _db.FuelControls.SingleOrDefaultAsync(x => x.Id == id, ct);

	public async Task Create(FuelControl control, CancellationToken ct = default)
		=> await _db.FuelControls.AddAsync(control, ct);

	public void Update(FuelControl control)
		=> _db.FuelControls.Update(control);

	public void Delete(FuelControl control)
		=> _db.FuelControls.Remove(control);
}
