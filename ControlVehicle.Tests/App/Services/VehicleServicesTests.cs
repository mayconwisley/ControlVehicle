using ControlVehicle.App.Services.Vehicle;
using ControlVehicle.Domain.Entities;
using ControlVehicle.Domain.Enums;
using ControlVehicle.Domain.Pagination;
using ControlVehicle.Domain.Repositories;
using ControlVehicle.Domain.ValueObjects;
using ControlVehicle.Models.Dtos;

namespace ControlVehicle.Tests.App.Services;

public class VehicleServicesTests
{
    [Fact]
    public async Task Create_ShouldPersistVehicle_AndCommit()
    {
        var repo = new FakeVehicleRepository();
        var uow = new FakeUnitOfWork();
        var service = new VehicleServices(repo, uow);
        var dto = new VehicleCreateDto(
            "12345678901",
            "Civic",
            "ABC1234",
            FuelEnum.Gasoline,
            null,
            VehicleColorEnum.Black,
            false);

        var created = await service.Create(dto);
        var all = await repo.GetAll(1, 10, string.Empty);

        Assert.Single(all.Items);
        Assert.Equal(created.Id, all.Items[0].Id);
        Assert.False(all.Items[0].Active);
        Assert.Equal(1, uow.CommitCalls);
    }

    [Fact]
    public async Task Update_ShouldChangeValues_AndCommit_WhenVehicleExists()
    {
        var repo = new FakeVehicleRepository();
        var uow = new FakeUnitOfWork();
        var service = new VehicleServices(repo, uow);
        var existing = new Vehicle(
            LicensePlate.Create("DEF1234"),
            "Focus",
            Renavam.Create("98765432100"),
            null,
            FuelEnum.Flex,
            VehicleColorEnum.White);
        await repo.Create(existing);

        var updated = new VehicleUpdateDto(
            existing.Id,
            "12345678901",
            "Civic",
            "ABC1234",
            FuelEnum.Gasoline,
            null,
            VehicleColorEnum.Black,
            false);

        var updatedResult = await service.Update(updated);
        var stored = await repo.GetById(existing.Id);

        Assert.NotNull(updatedResult);
        Assert.NotNull(stored);
        Assert.Equal(updated.LicensePlate, stored!.LicensePlate.Value);
        Assert.Equal(updated.Model, stored.Model);
        Assert.Equal(updated.Renavam, stored.Renavam.Value);
        Assert.Equal(updated.Fuel, stored.Fuel);
        Assert.Equal(updated.VehicleColor, stored.VehicleColor);
        Assert.False(stored.Active);
        Assert.Equal(1, uow.CommitCalls);
    }

    [Fact]
    public async Task Update_ShouldReturnNull_WhenVehicleDoesNotExist()
    {
        var repo = new FakeVehicleRepository();
        var uow = new FakeUnitOfWork();
        var service = new VehicleServices(repo, uow);
        var updated = new VehicleUpdateDto(
            Guid.NewGuid(),
            "12345678901",
            "Civic",
            "ABC1234",
            FuelEnum.Gasoline,
            null,
            VehicleColorEnum.Black,
            true);

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

    private sealed class FakeVehicleRepository : IVehicleRepository
    {
        private readonly List<Vehicle> _vehicles = [];

        public Task<PagedData<Vehicle>> GetAll(int page = 1, int size = 5, string? search = null, CancellationToken ct = default)
        {
            IEnumerable<Vehicle> query = _vehicles;
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(v => v.Model.Contains(search, StringComparison.OrdinalIgnoreCase));
            }

            var total = query.Count();
            var items = query.Skip((page - 1) * size).Take(size).ToList();
            return Task.FromResult(new PagedData<Vehicle>(items, total));
        }

        public Task<Vehicle?> GetById(Guid id, CancellationToken ct = default)
            => Task.FromResult(_vehicles.SingleOrDefault(v => v.Id == id));

        public Task<Vehicle?> GetByLicensePlate(LicensePlate licensePlate, CancellationToken ct = default)
            => Task.FromResult(_vehicles.SingleOrDefault(v => v.LicensePlate.Value == licensePlate.Value));

        public Task<Vehicle?> GetByRenavam(Renavam renavam, CancellationToken ct = default)
            => Task.FromResult(_vehicles.SingleOrDefault(v => v.Renavam.Value == renavam.Value));

        public Task Create(Vehicle vehicle, CancellationToken ct = default)
        {
            _vehicles.Add(vehicle);
            return Task.CompletedTask;
        }

        public void Update(Vehicle vehicle)
        {
            var index = _vehicles.FindIndex(v => v.Id == vehicle.Id);
            if (index >= 0)
            {
                _vehicles[index] = vehicle;
            }
        }

        public void Delete(Vehicle vehicle)
            => _vehicles.RemoveAll(v => v.Id == vehicle.Id);
    }
}
