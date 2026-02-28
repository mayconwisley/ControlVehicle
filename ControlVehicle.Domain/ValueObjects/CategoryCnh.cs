namespace ControlVehicle.Domain.ValueObjects;

public sealed class CategoryCnh : IEquatable<CategoryCnh>
{
	private static readonly HashSet<string> ValidCategories =
	[
		"A", "B", "C", "D", "E",
		"AB", "AC", "AD", "AE"
	];
	public string Value { get; private set; } = null!;

	private CategoryCnh() { }
	private CategoryCnh(string value)
	{
		Value = value;
	}
	public static CategoryCnh Create(string value)
	{
		if (string.IsNullOrWhiteSpace(value))
			throw new ArgumentException("A categoria não pode estar vazia.");

		value = value.ToUpperInvariant();

		if (!ValidCategories.Contains(value))
			throw new ArgumentException("Categoria CNH inválida.");

		return new CategoryCnh(value);
	}
	public override string ToString() => Value;
	public override bool Equals(object? obj) => Equals(obj as CategoryCnh);
	public bool Equals(CategoryCnh? other)
		=> other is not null && Value == other.Value;
	public override int GetHashCode()
		=> Value.GetHashCode();
}
