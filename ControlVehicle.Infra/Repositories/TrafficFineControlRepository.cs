using ControlVehicle.Domain.Entities;
using ControlVehicle.Domain.Pagination;
using ControlVehicle.Domain.Repositories;
using ControlVehicle.Infra.Database;
using Microsoft.EntityFrameworkCore;

namespace ControlVehicle.Infra.Repositories;

public sealed class TrafficFineControlRepository(VehicleDbContext db) : ITrafficFineControlRepository
{
    private readonly VehicleDbContext _db = db;

    public async Task<PagedData<TrafficFineControl>> GetAll(int page = 1, int size = 5, string? search = null, CancellationToken ct = default)
    {
        page = page < 1 ? 1 : page;
        size = size < 1 ? 5 : size;

        IQueryable<TrafficFineControl> query = _db.TrafficFineControls.AsNoTracking();

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

        return new PagedData<TrafficFineControl>(items, total);
    }

    public async Task<TrafficFineControl?> GetById(Guid id, CancellationToken ct = default)
        => await _db.TrafficFineControls.SingleOrDefaultAsync(x => x.Id == id, ct);

    public async Task Create(TrafficFineControl control, CancellationToken ct = default)
        => await _db.TrafficFineControls.AddAsync(control, ct);

    public void Update(TrafficFineControl control)
        => _db.TrafficFineControls.Update(control);

    public void Delete(TrafficFineControl control)
        => _db.TrafficFineControls.Remove(control);
}
