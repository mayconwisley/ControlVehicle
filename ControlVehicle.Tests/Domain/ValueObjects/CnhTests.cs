using ControlVehicle.Domain.ValueObjects;

namespace ControlVehicle.Tests.Domain.ValueObjects;

public class CnhTests
{
	[Fact]
	public void Create_ShouldNormalizeValidNumber()
	{
		var cnh = Cnh.Create("123.456.789-01");

		Assert.Equal("12345678901", cnh.Number);
	}

	[Fact]
	public void Create_ShouldThrow_WhenNumberIsInvalid()
	{
		Assert.Throws<ArgumentException>(() => Cnh.Create("123"));
	}
}
