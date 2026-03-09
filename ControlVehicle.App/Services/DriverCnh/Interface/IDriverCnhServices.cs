using ControlVehicle.Models.Dtos;

namespace ControlVehicle.App.Services.DriverCnh.Interface;

public interface IDriverCnhServices
{
	Task<IEnumerable<DriverCnhDto>> GetAll(int page, int size, string search);
	Task<DriverCnhDto?> GetByCnh(string cnh);
	Task<DriverCnhDto?> GetById(Guid id);
	Task Create(DriverCnhDto cnhDto);
	Task Update(DriverCnhDto cnhDto);
	Task Delete(string cnh);
	Task<int> Total();
}
