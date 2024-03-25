namespace ControlVehicle.Dto.Driver;

public class DriverDto
{
    public Guid Id { get; set; } = new Guid();
    public string? Name { get; set; } = string.Empty;
    public string? CNH { get; set; } = string.Empty;
    public string? CategoryCNH { get; set; } = string.Empty;
    public DateTime DateExpiration { get; set; } = new DateTime();
    public bool Active { get; set; } = true;
}
