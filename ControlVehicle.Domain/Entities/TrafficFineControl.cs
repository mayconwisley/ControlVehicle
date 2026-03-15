namespace ControlVehicle.Domain.Entities;

public class TrafficFineControl
{
    public const int DescriptionMaxLength = 500;

    public Guid Id { get; private set; }
    public Guid VehicleId { get; private set; }
    public Vehicle Vehicle { get; private set; } = null!;
    public Guid DriverId { get; private set; }
    public Driver Driver { get; private set; } = null!;
    public int Points { get; private set; }
    public decimal Value { get; private set; }
    public DateTime Date { get; private set; }
    public string? Description { get; private set; }

    protected TrafficFineControl() { } // EF Core

    public TrafficFineControl(
        Guid vehicleId,
        Guid driverId,
        int points,
        decimal value,
        DateTime date,
        string? description)
    {
        Validate(vehicleId, driverId, points, value, date, description);

        Id = Guid.NewGuid();
        VehicleId = vehicleId;
        DriverId = driverId;
        Points = points;
        Value = value;
        Date = date;
        Description = NormalizeDescription(description);
    }

    public void Update(
        Guid vehicleId,
        Guid driverId,
        int points,
        decimal value,
        DateTime date,
        string? description)
    {
        Validate(vehicleId, driverId, points, value, date, description);

        VehicleId = vehicleId;
        DriverId = driverId;
        Points = points;
        Value = value;
        Date = date;
        Description = NormalizeDescription(description);
    }

    private static string? NormalizeDescription(string? description)
        => string.IsNullOrWhiteSpace(description) ? null : description.Trim();

    private static void Validate(
        Guid vehicleId,
        Guid driverId,
        int points,
        decimal value,
        DateTime date,
        string? description)
    {
        if (vehicleId == Guid.Empty)
            throw new ArgumentException("O veículo é obrigatório.", nameof(vehicleId));

        if (driverId == Guid.Empty)
            throw new ArgumentException("O motorista é obrigatório.", nameof(driverId));

        if (points < 0)
            throw new ArgumentException("Os pontos não podem ser negativos.", nameof(points));

        if (value < 0)
            throw new ArgumentException("O valor não pode ser negativo.", nameof(value));

        if (date == default)
            throw new ArgumentException("A data é obrigatória.", nameof(date));

        if (!string.IsNullOrWhiteSpace(description) && description.Trim().Length > DescriptionMaxLength)
            throw new ArgumentException($"A descrição deve ter no máximo {DescriptionMaxLength} caracteres.", nameof(description));
    }
}
