using ControlVehicle.App.Services.FuelControl;
using ControlVehicle.Domain.Entities;
using ControlVehicle.Domain.Pagination;
using ControlVehicle.Domain.Repositories;
using ControlVehicle.Domain.ValueObjects;
using ControlVehicle.Models.Dtos;

namespace ControlVehicle.Tests.App.Services;

public class FuelControlServicesTests
{
    [Fact]
    public async Task GetById_ShouldReturnDto_WhenControlExists()
    {
        var repo = new FakeFuelControlRepository();
        var driverRepo = new FakeDriverRepository();
        var uow = new FakeUnitOfWork();
        var vehicleId = Guid.NewGuid();
        var driver = new Driver("Motorista", Cnh.Create("12345678901"), CategoryCnh.Create("B"), DateOnly.FromDateTime(DateTime.Today.AddYears(1)));
        await driverRepo.Create(driver);
        var existing = new FuelControl(
            vehicleId,
            driver.Id,
            1200.5m,
            450.75m,
            DateTime.UtcNow,
            45.2m,
            "Abastecimento");
        await repo.Create(existing);

        var service = new FuelControlServices(repo, driverRepo, uow);
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
        var driverRepo = new FakeDriverRepository();
        var uow = new FakeUnitOfWork();
        var driver = new Driver("Ana", Cnh.Create("98765432100"), CategoryCnh.Create("B"), DateOnly.FromDateTime(DateTime.Today.AddYears(1)));
        await driverRepo.Create(driver);
        var service = new FuelControlServices(repo, driverRepo, uow);
        var dto = new FuelControlCreateDto(
            Guid.NewGuid(),
            driver.Id,
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
    public async Task Create_ShouldThrow_WhenDriverCnhIsExpired()
    {
        var repo = new FakeFuelControlRepository();
        var driverRepo = new FakeDriverRepository();
        var uow = new FakeUnitOfWork();
        var expiredDriver = new Driver("Expirado", Cnh.Create("33333333333"), CategoryCnh.Create("B"), DateOnly.FromDateTime(DateTime.Today.AddDays(-1)));
        await driverRepo.Create(expiredDriver);
        var service = new FuelControlServices(repo, driverRepo, uow);
        var dto = new FuelControlCreateDto(
            Guid.NewGuid(),
            expiredDriver.Id,
            5000m,
            650.10m,
            DateTime.UtcNow,
            60.5m,
            null);

        var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => service.Create(dto));

        Assert.Contains("CNH do motorista esta vencida", ex.Message);
        Assert.Equal(0, uow.CommitCalls);
    }

    [Fact]
    public async Task Update_ShouldChangeValues_AndCommit_WhenControlExists()
    {
        var repo = new FakeFuelControlRepository();
        var driverRepo = new FakeDriverRepository();
        var uow = new FakeUnitOfWork();
        var oldDriver = new Driver("Antigo", Cnh.Create("11111111111"), CategoryCnh.Create("B"), DateOnly.FromDateTime(DateTime.Today.AddYears(1)));
        var newDriver = new Driver("Novo", Cnh.Create("22222222222"), CategoryCnh.Create("B"), DateOnly.FromDateTime(DateTime.Today.AddYears(1)));
        await driverRepo.Create(oldDriver);
        await driverRepo.Create(newDriver);
        var service = new FuelControlServices(repo, driverRepo, uow);
        var existing = new FuelControl(
            Guid.NewGuid(),
            oldDriver.Id,
            1200.5m,
            450.75m,
            DateTime.UtcNow.AddDays(-1),
            45.2m,
            "Abastecimento");
        await repo.Create(existing);

        var updated = new FuelControlUpdateDto(
            existing.Id,
            Guid.NewGuid(),
            newDriver.Id,
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
        var driverRepo = new FakeDriverRepository();
        var uow = new FakeUnitOfWork();
        var service = new FuelControlServices(repo, driverRepo, uow);
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

    private sealed class FakeDriverRepository : IDriverRepository
    {
        private readonly List<Driver> _drivers = [];

        public Task<PagedData<Driver>> GetAll(int page = 1, int size = 5, string? search = null, CancellationToken ct = default)
            => Task.FromResult(new PagedData<Driver>(_drivers, _drivers.Count));

        public Task<Driver?> GetById(Guid id, CancellationToken ct = default)
            => Task.FromResult(_drivers.SingleOrDefault(d => d.Id == id));

        public Task<Driver?> GetByCnh(Cnh cnh, CancellationToken ct = default)
            => Task.FromResult(_drivers.SingleOrDefault(d => d.Cnh.Number == cnh.Number));

        public Task Create(Driver driver, CancellationToken ct = default)
        {
            _drivers.Add(driver);
            return Task.CompletedTask;
        }

        public void Update(Driver driver)
        {
            var index = _drivers.FindIndex(d => d.Id == driver.Id);
            if (index >= 0)
            {
                _drivers[index] = driver;
            }
        }

        public void Delete(Driver driver)
            => _drivers.RemoveAll(d => d.Id == driver.Id);
    }
}
