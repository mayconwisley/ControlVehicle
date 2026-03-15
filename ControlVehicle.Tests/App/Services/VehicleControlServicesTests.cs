using ControlVehicle.App.Services.VehicleControl;
using ControlVehicle.Domain.Entities;
using ControlVehicle.Domain.Pagination;
using ControlVehicle.Domain.Repositories;
using ControlVehicle.Domain.ValueObjects;
using ControlVehicle.Models.Dtos;

namespace ControlVehicle.Tests.App.Services;

public class VehicleControlServicesTests
{
    [Fact]
    public async Task GetById_ShouldReturnDto_WhenControlExists()
    {
        var repo = new FakeVehicleControlRepository();
        var driverRepo = new FakeDriverRepository();
        var uow = new FakeUnitOfWork();
        var vehicleId = Guid.NewGuid();
        var driver = new Driver("Motorista", Cnh.Create("12345678901"), CategoryCnh.Create("B"), DateOnly.FromDateTime(DateTime.Today.AddYears(1)));
        await driverRepo.Create(driver);
        var existing = new VehicleControl(
            vehicleId,
            driver.Id,
            DateTime.UtcNow.AddHours(-2),
            DateTime.UtcNow.AddHours(-1),
            1200.5m,
            1210.8m,
            "Entrega de pecas");
        await repo.Create(existing);

        var service = new VehicleControlServices(repo, driverRepo, uow);
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
        var driverRepo = new FakeDriverRepository();
        var uow = new FakeUnitOfWork();
        var driver = new Driver("Ana", Cnh.Create("98765432100"), CategoryCnh.Create("B"), DateOnly.FromDateTime(DateTime.Today.AddYears(1)));
        await driverRepo.Create(driver);
        var service = new VehicleControlServices(repo, driverRepo, uow);
        var dto = new VehicleControlCreateDto(
            Guid.NewGuid(),
            driver.Id,
            DateTime.UtcNow.AddHours(-3),
            DateTime.UtcNow.AddHours(-1),
            5000m,
            5075.2m,
            "Visita tecnica");

        var created = await service.Create(dto);
        var all = await repo.GetAll(1, 10, string.Empty);

        Assert.Single(all.Items);
        Assert.Equal(created.Id, all.Items[0].Id);
        Assert.Equal(1, uow.CommitCalls);
    }

    [Fact]
    public async Task Create_ShouldThrow_WhenDriverCnhIsExpired()
    {
        var repo = new FakeVehicleControlRepository();
        var driverRepo = new FakeDriverRepository();
        var uow = new FakeUnitOfWork();
        var expiredDriver = new Driver("Expirado", Cnh.Create("33333333333"), CategoryCnh.Create("B"), DateOnly.FromDateTime(DateTime.Today.AddDays(-1)));
        await driverRepo.Create(expiredDriver);
        var service = new VehicleControlServices(repo, driverRepo, uow);
        var dto = new VehicleControlCreateDto(
            Guid.NewGuid(),
            expiredDriver.Id,
            DateTime.UtcNow.AddHours(-3),
            DateTime.UtcNow.AddHours(-1),
            5000m,
            5075.2m,
            "Viagem");

        var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => service.Create(dto));

        Assert.Contains("CNH do motorista esta vencida", ex.Message);
        Assert.Equal(0, uow.CommitCalls);
    }

    [Fact]
    public async Task Update_ShouldChangeValues_AndCommit_WhenControlExists()
    {
        var repo = new FakeVehicleControlRepository();
        var driverRepo = new FakeDriverRepository();
        var uow = new FakeUnitOfWork();
        var oldDriver = new Driver("Antigo", Cnh.Create("11111111111"), CategoryCnh.Create("B"), DateOnly.FromDateTime(DateTime.Today.AddYears(1)));
        var newDriver = new Driver("Novo", Cnh.Create("22222222222"), CategoryCnh.Create("B"), DateOnly.FromDateTime(DateTime.Today.AddYears(1)));
        await driverRepo.Create(oldDriver);
        await driverRepo.Create(newDriver);
        var service = new VehicleControlServices(repo, driverRepo, uow);
        var existing = new VehicleControl(
            Guid.NewGuid(),
            oldDriver.Id,
            DateTime.UtcNow.AddHours(-4),
            DateTime.UtcNow.AddHours(-2),
            1200.5m,
            1210.8m,
            "Entrega de pecas");
        await repo.Create(existing);

        var updated = new VehicleControlUpdateDto(
            existing.Id,
            Guid.NewGuid(),
            newDriver.Id,
            DateTime.UtcNow.AddHours(-1),
            DateTime.UtcNow,
            2000m,
            2050m,
            "Retorno");

        var updatedResult = await service.Update(updated);
        var stored = await repo.GetById(existing.Id);

        Assert.NotNull(updatedResult);
        Assert.NotNull(stored);
        Assert.Equal(updated.VehicleId, stored!.VehicleId);
        Assert.Equal(updated.DriverId, stored.DriverId);
        Assert.Equal(updated.DepartureDate, stored.DepartureDate);
        Assert.Equal(updated.ArrivalDate, stored.ArrivalDate);
        Assert.Equal(updated.InitialKm, stored.InitialKm);
        Assert.Equal(updated.FinalKm, stored.FinalKm);
        Assert.Equal(updated.Description, stored.Description);
        Assert.Equal(1, uow.CommitCalls);
    }

    [Fact]
    public async Task Update_ShouldNotCommit_WhenControlDoesNotExist()
    {
        var repo = new FakeVehicleControlRepository();
        var driverRepo = new FakeDriverRepository();
        var uow = new FakeUnitOfWork();
        var service = new VehicleControlServices(repo, driverRepo, uow);
        var updated = new VehicleControlUpdateDto(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            DateTime.UtcNow.AddHours(-1),
            DateTime.UtcNow,
            2000m,
            2050m,
            "Retorno");

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
