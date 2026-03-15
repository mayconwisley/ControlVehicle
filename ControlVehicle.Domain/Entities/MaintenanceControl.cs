namespace ControlVehicle.Domain.Entities;

public class MaintenanceControl
{
    public const int DescriptionMaxLength = 500;

    public Guid Id { get; private set; }
    public Guid VehicleId { get; private set; }
    public Vehicle Vehicle { get; private set; } = null!;
    public DateTime Date { get; private set; }
    public decimal Value { get; private set; }
    public string? Description { get; private set; }

    protected MaintenanceControl() { } // EF Core

    public MaintenanceControl(
        Guid vehicleId,
        DateTime date,
        decimal value,
        string? description)
    {
        Validate(vehicleId, date, value, description);

        Id = Guid.NewGuid();
        VehicleId = vehicleId;
        Date = date;
        Value = value;
        Description = NormalizeDescription(description);
    }

    public void Update(
        Guid vehicleId,
        DateTime date,
        decimal value,
        string? description)
    {
        Validate(vehicleId, date, value, description);

        VehicleId = vehicleId;
        Date = date;
        Value = value;
        Description = NormalizeDescription(description);
    }

    private static string? NormalizeDescription(string? description)
        => string.IsNullOrWhiteSpace(description) ? null : description.Trim();

    private static void Validate(
        Guid vehicleId,
        DateTime date,
        decimal value,
        string? description)
    {
        if (vehicleId == Guid.Empty)
            throw new ArgumentException("O veículo é obrigatório.", nameof(vehicleId));

        if (date == default)
            throw new ArgumentException("A data é obrigatória.", nameof(date));

        if (value < 0)
            throw new ArgumentException("O valor não pode ser negativo.", nameof(value));

        if (!string.IsNullOrWhiteSpace(description) && description.Trim().Length > DescriptionMaxLength)
            throw new ArgumentException($"A descrição deve ter no máximo {DescriptionMaxLength} caracteres.", nameof(description));
    }
}
