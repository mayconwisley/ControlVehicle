using ControlVehicle.Domain.Entities;
using ControlVehicle.Domain.Pagination;
using ControlVehicle.Domain.Repositories;
using ControlVehicle.Domain.ValueObjects;
using ControlVehicle.Infra.Database;
using Microsoft.EntityFrameworkCore;

namespace ControlVehicle.Infra.Repositories;

public sealed class DriverCnhRepository(VehicleDbContext db) : IDriverCnhRepository
{
	private readonly VehicleDbContext _db = db;

	public async Task<PagedData<DriverCnh>> GetAll(int page, int size, string? search = null, CancellationToken ct = default)
	{
		page = page < 1 ? 1 : page;
		size = size < 1 ? 10 : size;

		IQueryable<DriverCnh> query = _db.DriverCnhs.AsNoTracking();

		if (!string.IsNullOrWhiteSpace(search))
		{
			var pattern = $"%{search.Trim()}%";
			query = query.Where(x => EF.Functions.ILike(x.Cnh.Number, pattern));
		}

		var total = await query.CountAsync(ct);
		var items = await query
			.OrderBy(x => x.Cnh.Number)
			.Skip((page - 1) * size)
			.Take(size)
			.ToListAsync(ct);

		return new PagedData<DriverCnh>(items, total);
	}

	public Task<DriverCnh?> GetById(Guid id, CancellationToken ct = default)
		=> _db.DriverCnhs.SingleOrDefaultAsync(x => x.Id == id, ct);

	public Task<DriverCnh?> GetByCnh(Cnh cnh, CancellationToken ct = default)
		=> _db.DriverCnhs.SingleOrDefaultAsync(x => x.Cnh.Number == cnh.Number, ct);

	public Task<DriverCnh?> GetByDriverId(Guid driverId, CancellationToken ct = default)
		=> _db.DriverCnhs.SingleOrDefaultAsync(x => x.DriverId == driverId, ct);

	public Task Create(DriverCnh driverCnh, CancellationToken ct = default)
		=> _db.DriverCnhs.AddAsync(driverCnh, ct).AsTask();

	public void Update(DriverCnh driverCnh)
		=> _db.DriverCnhs.Update(driverCnh);

	public void Delete(DriverCnh driverCnh)
		=> _db.DriverCnhs.Remove(driverCnh);
}
