using System.Text.RegularExpressions;

namespace ControlVehicle.Domain.ValueObjects
{
	public sealed class LicensePlate
	{
		private static readonly Regex MercosulPattern =
			new(@"^[A-Z]{3}[0-9][A-Z][0-9]{2}$", RegexOptions.Compiled);

		private static readonly Regex OldPattern =
			new(@"^[A-Z]{3}[0-9]{4}$", RegexOptions.Compiled);

		public string Value { get; private set; } = null!;

		private LicensePlate() { } // EF

		private LicensePlate(string value) => Value = value;

		public static LicensePlate Create(string value)
		{
			if (string.IsNullOrWhiteSpace(value))
				throw new ArgumentException("A placa não pode estar vazia..");

			value = value.ToUpperInvariant().Replace("-", "");

			if (!MercosulPattern.IsMatch(value) &&
				!OldPattern.IsMatch(value))
				throw new ArgumentException("Formato de placa inválido.");

			return new LicensePlate(value);
		}

		public override string ToString() => Value;
	}
}
