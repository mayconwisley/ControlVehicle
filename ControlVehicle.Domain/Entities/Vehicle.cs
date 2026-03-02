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
	public VehicleColorEnum VehicleColor { get; private set; }

	public bool Active { get; private set; } = true;

	protected Vehicle() { } // EF Core

	public Vehicle(LicensePlate licensePlate, string model, Renavam renavam, Chassi? chassi, FuelEnum fuel, VehicleColorEnum vehiclecolor)
	{
		Validate(model, fuel, vehiclecolor);

		Id = Guid.NewGuid();
		LicensePlate = licensePlate ?? throw new ArgumentNullException(nameof(licensePlate));
		Model = model.Trim();
		Renavam = renavam ?? throw new ArgumentNullException(nameof(renavam));
		Chassi = chassi;
		Fuel = fuel;
		VehicleColor = vehiclecolor;
		Active = true;
	}

	public void Activate() => Active = true;
	public void Deactivate() => Active = false;

	public void Update(LicensePlate licensePlate, string model, Renavam renavam, Chassi? chassi, FuelEnum fuel, VehicleColorEnum vehiclecolor)
	{
		Validate(model, fuel, vehiclecolor);
		LicensePlate = licensePlate;
		Model = model.Trim();
		Renavam = renavam;
		Chassi = chassi;
		Fuel = fuel;
		VehicleColor = vehiclecolor;
	}

	private static void Validate(string model, FuelEnum fuel, VehicleColorEnum vehiclecolor)
	{
		if (string.IsNullOrWhiteSpace(model))
			throw new ArgumentException("O modelo não pode estar vazio.");
		if (fuel == FuelEnum.Unknown)
			throw new ArgumentException("O combustível é obrigatório.");

		if (vehiclecolor == VehicleColorEnum.Unknown)
			throw new ArgumentException("A cor do veículo é obrigatória.");
	}
}