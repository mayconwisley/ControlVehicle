using ControlVehicle.Domain.Entities;
using ControlVehicle.Domain.Pagination;
using ControlVehicle.Domain.Repositories;
using ControlVehicle.Infra.Database;
using Microsoft.EntityFrameworkCore;

namespace ControlVehicle.Infra.Repositories;

public sealed class VehicleControlRepository(VehicleDbContext db) : IVehicleControlRepository
{
	private readonly VehicleDbContext _db = db;

	public async Task<PagedData<VehicleControl>> GetAll(int page = 1, int size = 5, string? search = null, CancellationToken ct = default)
	{
		page = page < 1 ? 1 : page;
		size = size < 1 ? 5 : size;

		IQueryable<VehicleControl> query = _db.VehicleControls.AsNoTracking();

		if (!string.IsNullOrWhiteSpace(search))
		{
			var trimmed = search.Trim();
			var pattern = $"%{trimmed}%";
			var hasId = Guid.TryParse(trimmed, out var id);

			query = query.Where(x =>
				EF.Functions.ILike(x.Description, pattern) ||
				(hasId && (x.VehicleId == id || x.DriverId == id))
			);
		}

		var total = await query.CountAsync(ct);
		var items = await query
			.OrderByDescending(x => x.DepartureDate)
			.Skip((page - 1) * size)
			.Take(size)
			.ToListAsync(ct);

		return new PagedData<VehicleControl>(items, total);
	}

	public async Task<VehicleControl?> GetById(Guid id, CancellationToken ct = default)
		=> await _db.VehicleControls.SingleOrDefaultAsync(x => x.Id == id, ct);

	public async Task Create(VehicleControl control, CancellationToken ct = default)
		=> await _db.VehicleControls.AddAsync(control, ct);

	public void Update(VehicleControl control)
		=> _db.VehicleControls.Update(control);

	public void Delete(VehicleControl control)
		=> _db.VehicleControls.Remove(control);
}
