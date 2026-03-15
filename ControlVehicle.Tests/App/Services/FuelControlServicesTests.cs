using ControlVehicle.App.Services.FuelControl;
using ControlVehicle.Domain.Entities;
using ControlVehicle.Domain.Pagination;
using ControlVehicle.Domain.Repositories;
using ControlVehicle.Models.Dtos;

namespace ControlVehicle.Tests.App.Services;

public class FuelControlServicesTests
{
    [Fact]
    public async Task GetById_ShouldReturnDto_WhenControlExists()
    {
        var repo = new FakeFuelControlRepository();
        var uow = new FakeUnitOfWork();
        var vehicleId = Guid.NewGuid();
        var driverId = Guid.NewGuid();
        var existing = new FuelControl(
            vehicleId,
            driverId,
            1200.5m,
            450.75m,
            DateTime.UtcNow,
            45.2m,
            "Abastecimento");
        await repo.Create(existing);

        var service = new FuelControlServices(repo, uow);
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
        var repo = new FakeFuelControlRepository();
        var uow = new FakeUnitOfWork();
        var service = new FuelControlServices(repo, uow);
        var dto = new FuelControlCreateDto(
            Guid.NewGuid(),
            Guid.NewGuid(),
            5000m,
            650.10m,
            DateTime.UtcNow,
            60.5m,
            null);

        var created = await service.Create(dto);
        var all = await repo.GetAll(1, 10, string.Empty);

        Assert.Single(all.Items);
        Assert.Equal(created.Id, all.Items[0].Id);
        Assert.Equal(1, uow.CommitCalls);
    }

    [Fact]
    public async Task Update_ShouldChangeValues_AndCommit_WhenControlExists()
    {
        var repo = new FakeFuelControlRepository();
        var uow = new FakeUnitOfWork();
        var service = new FuelControlServices(repo, uow);
        var existing = new FuelControl(
            Guid.NewGuid(),
            Guid.NewGuid(),
            1200.5m,
            450.75m,
            DateTime.UtcNow.AddDays(-1),
            45.2m,
            "Abastecimento");
        await repo.Create(existing);

        var updated = new FuelControlUpdateDto(
            existing.Id,
            Guid.NewGuid(),
            Guid.NewGuid(),
            2000.1m,
            800.00m,
            DateTime.UtcNow,
            55.5m,
            "Completo");

        var updatedResult = await service.Update(updated);
        var stored = await repo.GetById(existing.Id);

        Assert.NotNull(updatedResult);
        Assert.NotNull(stored);
        Assert.Equal(updated.VehicleId, stored!.VehicleId);
        Assert.Equal(updated.DriverId, stored.DriverId);
        Assert.Equal(updated.InitialKm, stored.InitialKm);
        Assert.Equal(updated.Value, stored.Value);
        Assert.Equal(updated.Date, stored.Date);
        Assert.Equal(updated.Liters, stored.Liters);
        Assert.Equal(updated.Description, stored.Description);
        Assert.Equal(1, uow.CommitCalls);
    }

    [Fact]
    public async Task Update_ShouldNotCommit_WhenControlDoesNotExist()
    {
        var repo = new FakeFuelControlRepository();
        var uow = new FakeUnitOfWork();
        var service = new FuelControlServices(repo, uow);
        var updated = new FuelControlUpdateDto(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            2000.1m,
            800.00m,
            DateTime.UtcNow,
            55.5m,
            "Completo");

        var updatedResult = await service.Update(updated);

        Assert.Null(updatedResult);
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

    private sealed class FakeFuelControlRepository : IFuelControlRepository
    {
        private readonly List<FuelControl> _controls = [];

        public Task<PagedData<FuelControl>> GetAll(int page = 1, int size = 5, string? search = null, CancellationToken ct = default)
        {
            IEnumerable<FuelControl> query = _controls;
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(c => c.Description?.Contains(search, StringComparison.OrdinalIgnoreCase) == true);
            }

            var total = query.Count();
            var items = query.Skip((page - 1) * size).Take(size).ToList();
            return Task.FromResult(new PagedData<FuelControl>(items, total));
        }

        public Task<FuelControl?> GetById(Guid id, CancellationToken ct = default)
            => Task.FromResult(_controls.SingleOrDefault(c => c.Id == id));

        public Task Create(FuelControl control, CancellationToken ct = default)
        {
            _controls.Add(control);
            return Task.CompletedTask;
        }

        public void Update(FuelControl control)
        {
            var index = _controls.FindIndex(c => c.Id == control.Id);
            if (index >= 0)
            {
                _controls[index] = control;
            }
        }

        public void Delete(FuelControl control)
            => _controls.RemoveAll(c => c.Id == control.Id);
    }
}
