using ControlVehicle.Domain.Entities;
using ControlVehicle.Domain.Pagination;
using ControlVehicle.Domain.Repositories;
using ControlVehicle.Infra.Database;
using Microsoft.EntityFrameworkCore;

namespace ControlVehicle.Infra.Repositories;

public sealed class MaintenanceControlRepository(VehicleDbContext db) : IMaintenanceControlRepository
{
    private readonly VehicleDbContext _db = db;

    public async Task<PagedData<MaintenanceControl>> GetAll(int page = 1, int size = 5, string? search = null, CancellationToken ct = default)
    {
        page = page < 1 ? 1 : page;
        size = size < 1 ? 5 : size;

        IQueryable<MaintenanceControl> query = _db.MaintenanceControls.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var trimmed = search.Trim();
            var pattern = $"%{trimmed}%";
            var hasId = Guid.TryParse(trimmed, out var id);

            query = query.Where(x =>
                (x.Description != null && EF.Functions.ILike(x.Description, pattern)) ||
                (hasId && x.VehicleId == id)
            );
        }

        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(x => x.Date)
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync(ct);

        return new PagedData<MaintenanceControl>(items, total);
    }

    public async Task<MaintenanceControl?> GetById(Guid id, CancellationToken ct = default)
        => await _db.MaintenanceControls.SingleOrDefaultAsync(x => x.Id == id, ct);

    public async Task Create(MaintenanceControl control, CancellationToken ct = default)
        => await _db.MaintenanceControls.AddAsync(control, ct);

    public void Update(MaintenanceControl control)
        => _db.MaintenanceControls.Update(control);

    public void Delete(MaintenanceControl control)
        => _db.MaintenanceControls.Remove(control);
}
