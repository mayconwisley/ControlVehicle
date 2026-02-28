using System.Text.RegularExpressions;

namespace ControlVehicle.Domain.ValueObjects;

public sealed class Chassi
{
	// VIN: 17 chars, A-H J-N P R-Z 0-9 (exclui I,O,Q)
	private static readonly Regex VinPattern =
		new(@"^[A-HJ-NPR-Z0-9]{17}$", RegexOptions.Compiled);

	public string Value { get; private set; } = null!;

	private Chassi() { } // EF Core

	private Chassi(string value) => Value = value;

	public static Chassi Create(string? value)
	{
		if (string.IsNullOrWhiteSpace(value))
			throw new ArgumentException("O chassi não pode estar vazio.");

		value = value.Trim().ToUpperInvariant();

		// remove espaços internos se você quiser tolerância (opcional):
		value = value.Replace(" ", "");

		if (!VinPattern.IsMatch(value))
			throw new ArgumentException("Chassi inválido. Deve conter 17 caracteres alfanuméricos e não pode conter I, O, Q.");

		return new Chassi(value);
	}

	public override string ToString() => Value;
}
