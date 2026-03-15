using ControlVehicle.Models.Dtos;

namespace ControlVehicle.App.Services.FuelControl.Interface;

public interface IFuelControlServices
{
	Task<IEnumerable<FuelControlDto>> GetAll(int page, int size, string search);
	Task<FuelControlDto?> GetById(Guid id);
	Task<FuelControlDto> Create(FuelControlCreateDto control);
	Task<FuelControlDto?> Update(FuelControlUpdateDto control);
	Task Delete(Guid id);
	Task<int> TotalFuelControl();
}
