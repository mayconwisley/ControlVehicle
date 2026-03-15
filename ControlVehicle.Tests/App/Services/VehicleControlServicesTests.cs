using ControlVehicle.App.Services.VehicleControl;
using ControlVehicle.Domain.Entities;
using ControlVehicle.Domain.Pagination;
using ControlVehicle.Domain.Repositories;
using ControlVehicle.Models.Dtos;

namespace ControlVehicle.Tests.App.Services;

public class VehicleControlServicesTests
{
    [Fact]
    public async Task GetById_ShouldReturnDto_WhenControlExists()
    {
        var repo = new FakeVehicleControlRepository();
        var uow = new FakeUnitOfWork();
        var vehicleId = Guid.NewGuid();
        var driverId = Guid.NewGuid();
        var existing = new VehicleControl(
            vehicleId,
            driverId,
            DateTime.UtcNow.AddHours(-2),
            DateTime.UtcNow.AddHours(-1),
            1200.5m,
            1210.8m,
            "Entrega de peças");
        await repo.Create(existing);

        var service = new VehicleControlServices(repo, uow);
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
        var repo = new FakeVehicleControlRepository();
        var uow = new FakeUnitOfWork();
        var service = new VehicleControlServices(repo, uow);
        var dto = new VehicleControlDto(
            Guid.Empty,
            Guid.NewGuid(),
            Guid.NewGuid(),
            DateTime.UtcNow.AddHours(-3),
            DateTime.UtcNow.AddHours(-1),
            5000m,
            5075.2m,
            "Visita técnica");

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

    private sealed class FakeVehicleControlRepository : IVehicleControlRepository
    {
        private readonly List<VehicleControl> _controls = [];

        public Task<PagedData<VehicleControl>> GetAll(int page = 1, int size = 5, string? search = null, CancellationToken ct = default)
        {
            IEnumerable<VehicleControl> query = _controls;
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(c => c.Description.Contains(search, StringComparison.OrdinalIgnoreCase));
            }

            var total = query.Count();
            var items = query.Skip((page - 1) * size).Take(size).ToList();
            return Task.FromResult(new PagedData<VehicleControl>(items, total));
        }

        public Task<VehicleControl?> GetById(Guid id, CancellationToken ct = default)
            => Task.FromResult(_controls.SingleOrDefault(c => c.Id == id));

        public Task Create(VehicleControl control, CancellationToken ct = default)
        {
            _controls.Add(control);
            return Task.CompletedTask;
        }

        public void Update(VehicleControl control)
        {
            var index = _controls.FindIndex(c => c.Id == control.Id);
            if (index >= 0)
            {
                _controls[index] = control;
            }
        }

        public void Delete(VehicleControl control)
            => _controls.RemoveAll(c => c.Id == control.Id);
    }
}

