using ControlVehicle.Domain.ValueObjects;

namespace ControlVehicle.Domain.Entities;

public class Driver
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = null!;
    public Cnh Cnh { get; private set; } = null!;
    public CategoryCnh CategoryCnh { get; private set; } = null!;
    public DateOnly DateExpiration { get; private set; }
    public bool Active { get; private set; } = true;
    public ICollection<VehicleControl> Controls { get; private set; } = new List<VehicleControl>();
    public ICollection<FuelControl> FuelControls { get; private set; } = new List<FuelControl>();
    public ICollection<TrafficFineControl> TrafficFineControls { get; private set; } = new List<TrafficFineControl>();

    protected Driver() { } // EF Core

    public Driver(string name, Cnh cnh, CategoryCnh categoryCnh, DateOnly dateExpiration)
    {
        Validate(name, dateExpiration);

        Id = Guid.NewGuid();
        Name = name.Trim();
        Cnh = cnh ?? throw new ArgumentNullException(nameof(cnh));
        CategoryCnh = categoryCnh ?? throw new ArgumentNullException(nameof(categoryCnh));
        DateExpiration = dateExpiration;
        Active = true;
    }

    public void Activate() => Active = true;
    public void Deactivate() => Active = false;

    public void Update(string name, Cnh cnh, CategoryCnh categoryCnh, DateOnly dateExpiration)
    {
        Validate(name, dateExpiration);

        Name = name.Trim();
        Cnh = cnh ?? throw new ArgumentNullException(nameof(cnh));
        CategoryCnh = categoryCnh ?? throw new ArgumentNullException(nameof(categoryCnh));
        DateExpiration = dateExpiration;
    }

    private static void Validate(string name, DateOnly dateExpiration)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("O nome é obrigatório.", nameof(name));

        if (dateExpiration == default)
            throw new ArgumentException("A data de validade da CNH é obrigatória.", nameof(dateExpiration));
    }
}

