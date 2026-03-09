using System.ComponentModel;

namespace ControlVehicle.Domain.Enums;

public enum FuelEnum
{
	[Description("Desconhecido")]
	Unknown = 0,
	[Description("Gasolina")]
	Gasoline = 1,
	[Description("Diesel")]
	Diesel = 2,
	[Description("Etanol")]
	Ethanol = 3,
	[Description("Flex")]
	Flex = 4,
	[Description("Hibrido")]
	Hybrid = 5,
	[Description("Eletrico")]
	Electric = 6,
	[Description("Hidrogenio")]
	Hydrogen = 7,
	[Description("GNV")]
	Cng = 8,
	[Description("GLP")]
	Lpg = 9
}
