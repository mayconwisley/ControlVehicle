using ControlVehicle.App.Services.TrafficFineControl;
using ControlVehicle.Domain.Entities;
using ControlVehicle.Domain.Pagination;
using ControlVehicle.Domain.Repositories;
using ControlVehicle.Models.Dtos;

namespace ControlVehicle.Tests.App.Services;

public class TrafficFineControlServicesTests
{
    [Fact]
    public async Task GetById_ShouldReturnDto_WhenControlExists()
    {
        var repo = new FakeTrafficFineControlRepository();
        var uow = new FakeUnitOfWork();
        var vehicleId = Guid.NewGuid();
        var driverId = Guid.NewGuid();
        var existing = new TrafficFineControl(
            vehicleId,
            driverId,
            7,
            350.75m,
            DateTime.UtcNow,
            "Avanco de sinal");
        await repo.Create(existing);

        var service = new TrafficFineControlServices(repo, uow);
        var result = await service.GetById(existing.Id);

        Assert.NotNull(result);
        Assert.Equal(existing.Id, result!.Id);
        Assert.Equal(existing.VehicleId, result.VehicleId);
        Assert.Equal(existing.DriverId, result.DriverId);
        Assert.Equal(0, uow.CommitCalls);
    }

    [Fact]
    public async Task Create_ShouldPersistControl_AndCommit()
    {
        var repo = new FakeTrafficFineControlRepository();
        var uow = new FakeUnitOfWork();
        var service = new TrafficFineControlServices(repo, uow);
        var dto = new TrafficFineControlDto(
            Guid.Empty,
            Guid.NewGuid(),
            Guid.NewGuid(),
            4,
            195.10m,
            DateTime.UtcNow,
            null);

        var id = await service.Create(dto);
        var all = await repo.GetAll(1, 10, string.Empty);

        Assert.Single(all.Items);
        Assert.Equal(id, all.Items[0].Id);
        Assert.Equal(1, uow.CommitCalls);
    }

    [Fact]
    public async Task Update_ShouldChangeValues_AndCommit_WhenControlExists()
    {
        var repo = new FakeTrafficFineControlRepository();
        var uow = new FakeUnitOfWork();
        var service = new TrafficFineControlServices(repo, uow);
        var existing = new TrafficFineControl(
            Guid.NewGuid(),
            Guid.NewGuid(),
            3,
            120.50m,
            DateTime.UtcNow.AddDays(-2),
            "Velocidade");
        await repo.Create(existing);

        var updated = new TrafficFineControlDto(
            existing.Id,
            Guid.NewGuid(),
            Guid.NewGuid(),
            5,
            255.75m,
            DateTime.UtcNow,
            "Mudanca de faixa");

        await service.Update(updated);
        var stored = await repo.GetById(existing.Id);

        Assert.NotNull(stored);
        Assert.Equal(updated.VehicleId, stored!.VehicleId);
        Assert.Equal(updated.DriverId, stored.DriverId);
        Assert.Equal(updated.Points, stored.Points);
        Assert.Equal(updated.Value, stored.Value);
        Assert.Equal(updated.Date, stored.Date);
        Assert.Equal(updated.Description, stored.Description);
        Assert.Equal(1, uow.CommitCalls);
    }

    [Fact]
    public async Task Update_ShouldNotCommit_WhenControlDoesNotExist()
    {
        var repo = new FakeTrafficFineControlRepository();
        var uow = new FakeUnitOfWork();
        var service = new TrafficFineControlServices(repo, uow);
        var updated = new TrafficFineControlDto(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            2,
            99.90m,
            DateTime.UtcNow,
            null);

        await service.Update(updated);

        Assert.Equal(0, uow.CommitCalls);
    }

    [Fact]
    public async Task Delete_ShouldRemove_AndCommit_WhenControlExists()
    {
        var repo = new FakeTrafficFineControlRepository();
        var uow = new FakeUnitOfWork();
        var service = new TrafficFineControlServices(repo, uow);
        var existing = new TrafficFineControl(
            Guid.NewGuid(),
            Guid.NewGuid(),
            6,
            300.00m,
            DateTime.UtcNow.AddDays(-1),
            "Sinal vermelho");
        await repo.Create(existing);

        await service.Delete(existing.Id);
        var stored = await repo.GetById(existing.Id);

        Assert.Null(stored);
        Assert.Equal(1, uow.CommitCalls);
    }

    [Fact]
    public async Task Delete_ShouldNotCommit_WhenControlDoesNotExist()
    {
        var repo = new FakeTrafficFineControlRepository();
        var uow = new FakeUnitOfWork();
        var service = new TrafficFineControlServices(repo, uow);

        await service.Delete(Guid.NewGuid());

        Assert.Equal(0, uow.CommitCalls);
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

    private sealed class FakeTrafficFineControlRepository : ITrafficFineControlRepository
    {
        private readonly List<TrafficFineControl> _controls = [];

        public Task<PagedData<TrafficFineControl>> GetAll(int page = 1, int size = 5, string? search = null, CancellationToken ct = default)
        {
            IEnumerable<TrafficFineControl> query = _controls;
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(c => c.Description?.Contains(search, StringComparison.OrdinalIgnoreCase) == true);
            }

            var total = query.Count();
            var items = query.Skip((page - 1) * size).Take(size).ToList();
            return Task.FromResult(new PagedData<TrafficFineControl>(items, total));
        }

        public Task<TrafficFineControl?> GetById(Guid id, CancellationToken ct = default)
            => Task.FromResult(_controls.SingleOrDefault(c => c.Id == id));

        public Task Create(TrafficFineControl control, CancellationToken ct = default)
        {
            _controls.Add(control);
            return Task.CompletedTask;
        }

        public void Update(TrafficFineControl control)
        {
            var index = _controls.FindIndex(c => c.Id == control.Id);
            if (index >= 0)
            {
                _controls[index] = control;
            }
        }

        public void Delete(TrafficFineControl control)
            => _controls.RemoveAll(c => c.Id == control.Id);
    }
}
