using ControlVehicle.App.Services.MaintenanceControl;
using ControlVehicle.Domain.Entities;
using ControlVehicle.Domain.Pagination;
using ControlVehicle.Domain.Repositories;
using ControlVehicle.Models.Dtos;

namespace ControlVehicle.Tests.App.Services;

public class MaintenanceControlServicesTests
{
    [Fact]
    public async Task GetById_ShouldReturnDto_WhenControlExists()
    {
        var repo = new FakeMaintenanceControlRepository();
        var uow = new FakeUnitOfWork();
        var vehicleId = Guid.NewGuid();
        var existing = new MaintenanceControl(
            vehicleId,
            DateTime.UtcNow,
            900.50m,
            "Troca de óleo");
        await repo.Create(existing);

        var service = new MaintenanceControlServices(repo, uow);
        var result = await service.GetById(existing.Id);

        Assert.NotNull(result);
        Assert.Equal(existing.Id, result!.Id);
        Assert.Equal(existing.VehicleId, result.VehicleId);
        Assert.Equal(0, uow.CommitCalls);
    }

    [Fact]
    public async Task Create_ShouldPersistControl_AndCommit()
    {
        var repo = new FakeMaintenanceControlRepository();
        var uow = new FakeUnitOfWork();
        var service = new MaintenanceControlServices(repo, uow);
        var dto = new MaintenanceControlDto(
            Guid.Empty,
            Guid.NewGuid(),
            DateTime.UtcNow,
            1200.75m,
            null);

        var id = await service.Create(dto);
        var all = await repo.GetAll(1, 10, string.Empty);

        Assert.Single(all.Items);
        Assert.Equal(id, all.Items[0].Id);
        Assert.Equal(1, uow.CommitCalls);
    }

    private sealed class FakeUnitOfWork : IUnitOfWork
    {
        public int CommitCalls { get; private set; }

        public Task<int> CommitAsync(CancellationToken ct = default)
        {
            CommitCalls++;
            return Task.FromResult(1);
        }
    }

    private sealed class FakeMaintenanceControlRepository : IMaintenanceControlRepository
    {
        private readonly List<MaintenanceControl> _controls = [];

        public Task<PagedData<MaintenanceControl>> GetAll(int page = 1, int size = 5, string? search = null, CancellationToken ct = default)
        {
            IEnumerable<MaintenanceControl> query = _controls;
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(c => c.Description?.Contains(search, StringComparison.OrdinalIgnoreCase) == true);
            }

            var total = query.Count();
            var items = query.Skip((page - 1) * size).Take(size).ToList();
            return Task.FromResult(new PagedData<MaintenanceControl>(items, total));
        }

        public Task<MaintenanceControl?> GetById(Guid id, CancellationToken ct = default)
            => Task.FromResult(_controls.SingleOrDefault(c => c.Id == id));

        public Task Create(MaintenanceControl control, CancellationToken ct = default)
        {
            _controls.Add(control);
            return Task.CompletedTask;
        }

        public void Update(MaintenanceControl control)
        {
            var index = _controls.FindIndex(c => c.Id == control.Id);
            if (index >= 0)
            {
                _controls[index] = control;
            }
        }

        public void Delete(MaintenanceControl control)
            => _controls.RemoveAll(c => c.Id == control.Id);
    }
}
