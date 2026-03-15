using ControlVehicle.Domain.Entities;
using ControlVehicle.Domain.Pagination;

namespace ControlVehicle.Domain.Repositories;

public interface ITrafficFineControlRepository
{
    Task<PagedData<TrafficFineControl>> GetAll(int page = 1, int size = 5, string? search = null, CancellationToken ct = default);
    Task<TrafficFineControl?> GetById(Guid id, CancellationToken ct = default);
    Task Create(TrafficFineControl control, CancellationToken ct = default);
    void Update(TrafficFineControl control);
    void Delete(TrafficFineControl control);
}
