using ControlVehicle.Domain.Repositories;
using ControlVehicle.Infra.Database;

namespace ControlVehicle.Infra;

public sealed class UnitOfWork(VehicleDbContext db) : IUnitOfWork
{
	private readonly VehicleDbContext _db = db;

	public Task<int> CommitAsync(CancellationToken ct = default)
		=> _db.SaveChangesAsync(ct);
}
