using System.ComponentModel;

namespace ControlVehicle.Domain.Enums;

public enum VehicleColorEnum
{
	[Description("Desconhecido")]
	Unknown = 0,
	[Description("Branco")]
	White = 1,
	[Description("Preto")]
	Black = 2,
	[Description("Cinza")]
	Gray = 3,
	[Description("Prata")]
	Silver = 4,
	[Description("Azul")]
	Blue = 5,
	[Description("Vermelho")]
	Red = 6,
	[Description("Marrom")]
	Brown = 7,
	[Description("Verde")]
	Green = 8,
	[Description("Bege")]
	Beige = 9
}
