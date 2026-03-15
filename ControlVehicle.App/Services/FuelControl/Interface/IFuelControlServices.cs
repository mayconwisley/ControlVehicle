using ControlVehicle.Models.Dtos;

namespace ControlVehicle.App.Services.FuelControl.Interface;

public interface IFuelControlServices
{
	Task<IEnumerable<FuelControlDto>> GetAll(int page, int size, string search);
	Task<FuelControlDto?> GetById(Guid id);
	Task<Guid> Create(FuelControlDto control);
	Task Update(FuelControlDto control);
	Task Delete(Guid id);
	Task<int> TotalFuelControl();
}
