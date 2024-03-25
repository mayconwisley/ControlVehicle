using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlVehicle.Dto.Vehicle;

public class VehicleDto
{
    public Guid Id { get; set; } = new Guid();
    public string? Plate { get; set; } = string.Empty;
    public string? Model { get; set; } = string.Empty;
    public string? Chassi { get; set; } = string.Empty;
    public string? Renavam { get; set; } = string.Empty;
    public string? Fuel { get; set; } = string.Empty;
    public string? Color { get; set; } = string.Empty;
    public bool Active { get; set; } = true;
}
