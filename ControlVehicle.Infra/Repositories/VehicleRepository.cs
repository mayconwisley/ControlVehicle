using ControlVehicle.Domain.Entities;
using ControlVehicle.Domain.Pagination;
using ControlVehicle.Domain.Repositories;
using ControlVehicle.Domain.ValueObjects;
using ControlVehicle.Infra.Database;
using Microsoft.EntityFrameworkCore;

namespace ControlVehicle.Infra.Repositories;

public sealed class VehicleRepository(VehicleDbContext db) : IVehicleRepository
{
	private readonly VehicleDbContext _db = db;

	public async Task<PagedData<Vehicle>> GetAll(int page = 1, int size = 5, string? search = null, CancellationToken ct = default)
	{
		page = page < 1 ? 1 : page;
		size = size < 1 ? 5 : size;

		IQueryable<Vehicle> query = _db.Vehicles.AsNoTracking();

		if (!string.IsNullOrWhiteSpace(search))
		{
			var pattern = $"%{search.Trim()}%";
			query = _db.Vehicles
				.FromSqlInterpolated($@"
					SELECT *
					FROM control_vehicle.vehicles v
					WHERE v.""Model"" ILIKE {pattern}
					   OR v.""LicensePlate"" ILIKE {pattern}
					   OR v.""Renavam"" ILIKE {pattern}
					   OR COALESCE(v.""Chassi"", '') ILIKE {pattern}
					   OR CAST(v.""Id"" AS text) ILIKE {pattern}")
				.AsNoTracking();
		}

		var total = await query.CountAsync(ct);
		var items = await query
			.OrderBy(v => v.Model)
			.Skip((page - 1) * size)
			.Take(size)
			.ToListAsync(ct);

		return new PagedData<Vehicle>(items, total);
	}

	public async Task<Vehicle?> GetById(Guid id, CancellationToken ct = default)
		=> await _db.Vehicles.SingleOrDefaultAsync(v => v.Id == id, ct);

	public async Task<Vehicle?> GetByLicensePlate(LicensePlate licensePlate, CancellationToken ct = default)
		=> await _db.Vehicles.SingleOrDefaultAsync(v => v.LicensePlate.Value == licensePlate.Value, ct);

	public async Task<Vehicle?> GetByRenavam(Renavam renavam, CancellationToken ct = default)
		=> await _db.Vehicles.SingleOrDefaultAsync(v => v.Renavam.Value == renavam.Value, ct);

	public async Task Create(Vehicle vehicle, CancellationToken ct = default)
		=> await _db.Vehicles.AddAsync(vehicle, ct);

	public void Update(Vehicle vehicle)
		=> _db.Vehicles.Update(vehicle);

	public void Delete(Vehicle vehicle)
		=> _db.Vehicles.Remove(vehicle);
}
