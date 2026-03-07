using ControlVehicle.Domain.ValueObjects;

namespace ControlVehicle.Tests.Domain.ValueObjects;

public class LicensePlateTests
{
	[Fact]
	public void Create_ShouldAcceptMercosulFormat()
	{
		var plate = LicensePlate.Create("abc1d23");

		Assert.Equal("ABC1D23", plate.Value);
	}

	[Fact]
	public void Create_ShouldThrow_WhenFormatIsInvalid()
	{
		Assert.Throws<ArgumentException>(() => LicensePlate.Create("AAA111"));
	}
}
