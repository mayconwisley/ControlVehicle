using ControlVehicle.Domain.Enums;
using ControlVehicle.Domain.ValueObjects;

namespace ControlVehicle.Domain.Entities;

public class Vehicle
{
	public Guid Id { get; private set; }
	public LicensePlate LicensePlate { get; private set; } = null!;
	public string Model { get; private set; } = null!;
	public Renavam Renavam { get; private set; } = null!;
	public Chassi? Chassi { get; private set; }
	public FuelEnum Fuel { get; private set; }
	public VehicleColorEnum ColorVehicle { get; private set; }

	public bool Active { get; private set; } = true;

	protected Vehicle() { } // EF Core

	public Vehicle(LicensePlate licensePlate, string model, Renavam renavam, Chassi? chassi, FuelEnum fuel, VehicleColorEnum colorVehicle)
	{
		Validate(model, fuel, colorVehicle);

		Id = Guid.NewGuid();
		LicensePlate = licensePlate ?? throw new ArgumentNullException(nameof(licensePlate));
		Model = model.Trim();
		Renavam = renavam ?? throw new ArgumentNullException(nameof(renavam));
		Chassi = chassi;
		Fuel = fuel;
		ColorVehicle = colorVehicle;
		Active = true;
	}

	public void Activate() => Active = true;
	public void Deactivate() => Active = false;

	public void Update(string model, Chassi? chassi, FuelEnum fuel, VehicleColorEnum colorVehicle)
	{
		Validate(model, fuel, colorVehicle);

		Model = model.Trim();
		Chassi = chassi;
		Fuel = fuel;
		ColorVehicle = colorVehicle;
	}

	private static void Validate(string model, FuelEnum fuel, VehicleColorEnum colorVehicle)
	{
		if (string.IsNullOrWhiteSpace(model))
			throw new ArgumentException("O modelo não pode estar vazio.");
		if (fuel == FuelEnum.Unknown)
			throw new ArgumentException("O combustível é obrigatório.");

		if (colorVehicle == VehicleColorEnum.Unknown)
			throw new ArgumentException("A cor do veículo é obrigatória.");
	}
}