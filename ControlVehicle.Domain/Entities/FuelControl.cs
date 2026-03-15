namespace ControlVehicle.Domain.Entities;

public class FuelControl
{
    public const int DescriptionMaxLength = 500;

    public Guid Id { get; private set; }
    public Guid VehicleId { get; private set; }
    public Vehicle Vehicle { get; private set; } = null!;
    public Guid DriverId { get; private set; }
    public Driver Driver { get; private set; } = null!;
    public decimal InitialKm { get; private set; }
    public decimal Value { get; private set; }
    public DateTime Date { get; private set; }
    public decimal Liters { get; private set; }
    public string? Description { get; private set; }

    protected FuelControl() { } // EF Core

    public FuelControl(
        Guid vehicleId,
        Guid driverId,
        decimal initialKm,
        decimal value,
        DateTime date,
        decimal liters,
        string? description)
    {
        Validate(vehicleId, driverId, initialKm, value, date, liters, description);

        Id = Guid.NewGuid();
        VehicleId = vehicleId;
        DriverId = driverId;
        InitialKm = initialKm;
        Value = value;
        Date = date;
        Liters = liters;
        Description = NormalizeDescription(description);
    }

    public void Update(
        Guid vehicleId,
        Guid driverId,
        decimal initialKm,
        decimal value,
        DateTime date,
        decimal liters,
        string? description)
    {
        Validate(vehicleId, driverId, initialKm, value, date, liters, description);

        VehicleId = vehicleId;
        DriverId = driverId;
        InitialKm = initialKm;
        Value = value;
        Date = date;
        Liters = liters;
        Description = NormalizeDescription(description);
    }

    private static string? NormalizeDescription(string? description)
        => string.IsNullOrWhiteSpace(description) ? null : description.Trim();

    private static void Validate(
        Guid vehicleId,
        Guid driverId,
        decimal initialKm,
        decimal value,
        DateTime date,
        decimal liters,
        string? description)
    {
        if (vehicleId == Guid.Empty)
            throw new ArgumentException("O veículo é obrigatório.", nameof(vehicleId));

        if (driverId == Guid.Empty)
            throw new ArgumentException("O motorista é obrigatório.", nameof(driverId));

        if (initialKm < 0)
            throw new ArgumentException("O km inicial não pode ser negativo.", nameof(initialKm));

        if (value < 0)
            throw new ArgumentException("O valor não pode ser negativo.", nameof(value));

        if (date == default)
            throw new ArgumentException("A data é obrigatória.", nameof(date));

        if (liters <= 0)
            throw new ArgumentException("Os litros devem ser maiores que zero.", nameof(liters));

        if (!string.IsNullOrWhiteSpace(description) && description.Trim().Length > DescriptionMaxLength)
            throw new ArgumentException($"A descrição deve ter no máximo {DescriptionMaxLength} caracteres.", nameof(description));
    }
}
