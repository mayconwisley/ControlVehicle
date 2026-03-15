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
	public ICollection<VehicleControl> Controls { get; private set; } = new List<VehicleControl>();
	public ICollection<FuelControl> FuelControls { get; private set; } = new List<FuelControl>();

	protected Vehicle() { } // EF Core

	public Vehicle(
		LicensePlate licensePlate,
		string model,
		Renavam renavam,
		Chassi? chassi,
		FuelEnum fuel,
		VehicleColorEnum vehicleColor)
	{
		Validate(model, fuel, vehicleColor);

		Id = Guid.NewGuid();
		LicensePlate = licensePlate ?? throw new ArgumentNullException(nameof(licensePlate));
		Model = model.Trim();
		Renavam = renavam ?? throw new ArgumentNullException(nameof(renavam));
		Chassi = chassi;
		Fuel = fuel;
		VehicleColor = vehicleColor;
		Active = true;
	}

	public void Activate() => Active = true;
	public void Deactivate() => Active = false;

	public void Update(
		LicensePlate licensePlate,
		string model,
		Renavam renavam,
		Chassi? chassi,
		FuelEnum fuel,
		VehicleColorEnum vehicleColor)
	{
		Validate(model, fuel, vehicleColor);

		LicensePlate = licensePlate ?? throw new ArgumentNullException(nameof(licensePlate));
		Model = model.Trim();
		Renavam = renavam ?? throw new ArgumentNullException(nameof(renavam));
		Chassi = chassi;
		Fuel = fuel;
		VehicleColor = vehicleColor;
	}

	private static void Validate(string model, FuelEnum fuel, VehicleColorEnum vehicleColor)
	{
		if (string.IsNullOrWhiteSpace(model))
			throw new ArgumentException("O modelo não pode estar vazio.", nameof(model));

		if (fuel == FuelEnum.Unknown)
			throw new ArgumentException("O combustível é obrigatório.", nameof(fuel));

		if (vehicleColor == VehicleColorEnum.Unknown)
			throw new ArgumentException("A cor do veículo é obrigatória.", nameof(vehicleColor));
	}
}
