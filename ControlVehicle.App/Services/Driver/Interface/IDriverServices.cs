using ControlVehicle.Models.Dtos;

namespace ControlVehicle.App.Services.Driver.Interface;

public interface IDriverServices
{
	public Task<IEnumerable<DriverDto>> GetAll(int page, int size, string search);
	public Task<DriverDto?> GetByCnh(string cnh);
	public Task<DriverDto> Create(DriverCreateDto driver);
	public Task<DriverDto?> Update(DriverUpdateDto driver);
	public Task Delete(string cnh);
	public Task<int> TotalDriver();
}
