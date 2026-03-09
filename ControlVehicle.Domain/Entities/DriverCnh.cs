using ControlVehicle.Domain.Enums;
using ControlVehicle.Domain.ValueObjects;

namespace ControlVehicle.Domain.Entities;

public class DriverCnh
{
	public Guid Id { get; private set; }
	public Guid DriverId { get; private set; }
	public Cnh Cnh { get; private set; } = null!;
	public DateOnly DateExpiration { get; private set; }
	public EmploymentStatusEnum Status { get; private set; }

	protected DriverCnh() { }

	public DriverCnh(Guid driverId, Cnh cnh, DateOnly dateExpiration, EmploymentStatusEnum status)
	{
		if (driverId == Guid.Empty)
			throw new ArgumentException("DriverId invalido.", nameof(driverId));

		Id = Guid.NewGuid();
		DriverId = driverId;
		Cnh = cnh ?? throw new ArgumentNullException(nameof(cnh));
		DateExpiration = dateExpiration;
		Status = status;
	}

	public void Update(Guid driverId, Cnh cnh, DateOnly dateExpiration, EmploymentStatusEnum status)
	{
		if (driverId == Guid.Empty)
			throw new ArgumentException("DriverId invalido.", nameof(driverId));

		DriverId = driverId;
		Cnh = cnh ?? throw new ArgumentNullException(nameof(cnh));
		DateExpiration = dateExpiration;
		Status = status;
	}
}
