using ControlVehicle.App.Services.Driver;
using ControlVehicle.Domain.Entities;
using ControlVehicle.Domain.Pagination;
using ControlVehicle.Domain.Repositories;
using ControlVehicle.Domain.ValueObjects;
using ControlVehicle.Models.Dtos;

namespace ControlVehicle.Tests.App.Services;

public class DriverServicesTests
{
	[Fact]
	public async Task GetByCnh_ShouldReturnDto_WhenDriverExists()
	{
		var repo = new FakeDriverRepository();
		var uow = new FakeUnitOfWork();
		var existing = new Driver("Maycon", Cnh.Create("12345678901"), CategoryCnh.Create("B"), DateOnly.FromDateTime(DateTime.UtcNow.AddYears(2)));
		await repo.Create(existing);

		var service = new DriverServices(repo, uow);
		var result = await service.GetByCnh("12345678901");

		Assert.NotNull(result);
		Assert.Equal(existing.Cnh.Number, result!.Cnh);
		Assert.Equal(0, uow.CommitCalls);
	}

	[Fact]
	public async Task Create_ShouldPersistDriver_AndCommit()
	{
		var repo = new FakeDriverRepository();
		var uow = new FakeUnitOfWork();
		var service = new DriverServices(repo, uow);
		var dto = new DriverDto(
			Guid.NewGuid(),
			"Ana",
			"98765432100",
			"A",
			DateOnly.FromDateTime(DateTime.UtcNow.AddYears(1)),
			true);

		await service.Create(dto);
		var all = await repo.GetAll(1, 10, string.Empty);

		Assert.Single(all.Items);
		Assert.Equal("Ana", all.Items[0].Name);
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

	private sealed class FakeDriverRepository : IDriverRepository
	{
		private readonly List<Driver> _drivers = [];

		public Task<PagedData<Driver>> GetAll(int page = 1, int size = 5, string? search = null, CancellationToken ct = default)
		{
			IEnumerable<Driver> query = _drivers;
			if (!string.IsNullOrWhiteSpace(search))
			{
				query = query.Where(d => d.Name.Contains(search, StringComparison.OrdinalIgnoreCase));
			}

			var total = query.Count();
			var items = query.Skip((page - 1) * size).Take(size).ToList();
			return Task.FromResult(new PagedData<Driver>(items, total));
		}

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
