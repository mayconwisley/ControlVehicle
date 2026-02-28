namespace ControlVehicle.Domain.ValueObjects;

public sealed class Renavam
{
	public string Value { get; private set; } = null!;
	private Renavam() { }
	private Renavam(string value) => Value = value;
	public static Renavam Create(string value)
	{
		if (string.IsNullOrWhiteSpace(value))
			throw new ArgumentException("Renavam não pode estar vazio.");

		value = new string([.. value.Where(char.IsDigit)]);

		if (value.Length != 11)
			throw new ArgumentException("Renavam deve conter 11 dígitos.");

		return new Renavam(value);
	}

	public override string ToString() => Value;
}
