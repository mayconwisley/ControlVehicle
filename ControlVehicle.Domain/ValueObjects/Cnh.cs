using System.Text.RegularExpressions;

namespace ControlVehicle.Domain.ValueObjects;

public sealed class Cnh : IEquatable<Cnh>
{
	public string Number { get; private set; } = null!;

	private Cnh() { }
	private Cnh(string number)
	{
		Number = number;
	}

	public static Cnh Create(string number)
	{
		if (string.IsNullOrWhiteSpace(number))
			throw new ArgumentException("Cnh não pode estar vazio.");

		number = Regex.Replace(number, @"\D", "");

		if (number.Length != 11)
			throw new ArgumentException("Cnh deve conter 11 dígitos.");

		return new Cnh(number);
	}

	public override string ToString() => Number;

	public override bool Equals(object? obj) => Equals(obj as Cnh);

	public bool Equals(Cnh? other)
		=> other is not null && Number == other.Number;

	public override int GetHashCode()
		=> Number.GetHashCode();

	public static bool operator ==(Cnh left, Cnh right)
		=> Equals(left, right);

	public static bool operator !=(Cnh left, Cnh right)
		=> !Equals(left, right);
}
