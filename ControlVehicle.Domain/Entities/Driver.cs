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

	protected Driver() { }

	public Driver(string name, Cnh cnh, CategoryCnh categoryCnh, DateOnly dateExpiration)
	{
		Validation(name);

		Id = Guid.NewGuid();
		Name = name;
		Cnh = cnh ?? throw new ArgumentNullException(nameof(cnh));
		CategoryCnh = categoryCnh ?? throw new ArgumentNullException(nameof(categoryCnh));
		DateExpiration = dateExpiration;
		Active = true;
	}
	public void Activate() => Active = true;
	public void Deactivate() => Active = false;
	public void Update(string name, Cnh cnh, CategoryCnh categoryCnh, DateOnly dateExpiration)
	{
		Validation(name);

		Name = name;
		Cnh = cnh ?? throw new ArgumentNullException(nameof(cnh)); ;
		CategoryCnh = categoryCnh ?? throw new ArgumentNullException(nameof(categoryCnh));
		DateExpiration = dateExpiration;
	}

	private static void Validation(string name)
	{
		if (string.IsNullOrWhiteSpace(name))
			throw new ArgumentException("Name cannot be empty.", nameof(name));

	}
}
