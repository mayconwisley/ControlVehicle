namespace ControlVehicle.Domain.Entities;

public class VehicleControl
{
	public const int DescriptionMaxLength = 1000;

	public Guid Id { get; private set; }
	public Guid VehicleId { get; private set; }
	public Vehicle Vehicle { get; private set; } = null!;
	public Guid DriverId { get; private set; }
	public Driver Driver { get; private set; } = null!;
	public DateTime DepartureDate { get; private set; }
	public DateTime ArrivalDate { get; private set; }
	public decimal InitialKm { get; private set; }
	public decimal FinalKm { get; private set; }
	public string Description { get; private set; } = null!;

	protected VehicleControl() { } // EF Core

	public VehicleControl(
		Guid vehicleId,
		Guid driverId,
		DateTime departureDate,
		DateTime arrivalDate,
		decimal initialKm,
		decimal finalKm,
		string description)
	{
		Validate(vehicleId, driverId, departureDate, arrivalDate, initialKm, finalKm, description);

		Id = Guid.NewGuid();
		VehicleId = vehicleId;
		DriverId = driverId;
		DepartureDate = departureDate;
		ArrivalDate = arrivalDate;
		InitialKm = initialKm;
		FinalKm = finalKm;
		Description = NormalizeDescription(description);
	}

	public void Update(
		Guid vehicleId,
		Guid driverId,
		DateTime departureDate,
		DateTime arrivalDate,
		decimal initialKm,
		decimal finalKm,
		string description)
	{
		Validate(vehicleId, driverId, departureDate, arrivalDate, initialKm, finalKm, description);

		VehicleId = vehicleId;
		DriverId = driverId;
		DepartureDate = departureDate;
		ArrivalDate = arrivalDate;
		InitialKm = initialKm;
		FinalKm = finalKm;
		Description = NormalizeDescription(description);
	}

	private static string NormalizeDescription(string description) => description.Trim();

	private static void Validate(
		Guid vehicleId,
		Guid driverId,
		DateTime departureDate,
		DateTime arrivalDate,
		decimal initialKm,
		decimal finalKm,
		string description)
	{
		if (vehicleId == Guid.Empty)
			throw new ArgumentException("O veículo é obrigatório.", nameof(vehicleId));

		if (driverId == Guid.Empty)
			throw new ArgumentException("O motorista é obrigatório.", nameof(driverId));

		if (departureDate == default)
			throw new ArgumentException("A data de saída é obrigatória.", nameof(departureDate));

		if (arrivalDate == default)
			throw new ArgumentException("A data de chegada é obrigatória.", nameof(arrivalDate));

		if (arrivalDate < departureDate)
			throw new ArgumentException("A data de chegada não pode ser anterior à data de saída.", nameof(arrivalDate));

		if (initialKm < 0)
			throw new ArgumentException("O km inicial não pode ser negativo.", nameof(initialKm));

		if (finalKm < initialKm)
			throw new ArgumentException("O km final não pode ser menor que o km inicial.", nameof(finalKm));

		if (string.IsNullOrWhiteSpace(description))
			throw new ArgumentException("A descrição é obrigatória.", nameof(description));

		if (description.Trim().Length > DescriptionMaxLength)
			throw new ArgumentException($"A descrição deve ter no máximo {DescriptionMaxLength} caracteres.", nameof(description));
	}
}
