using ControlVehicle.Models.Dtos;

namespace ControlVehicle.App.Services.Driver.Interface;

public interface IDriverServices
{
	public Task<IEnumerable<DriverDto>> GetAll(int page, int size, string search);
	public Task<DriverDto> GetByCnh(string cnh);
	public Task Create(DriverDto driver);
	public Task Update(DriverDto driver);
	public Task Delete(string cnh);
	public Task<int> TotalDriver();
}
