using ControlVehicle.Models.Dtos;

namespace ControlVehicle.App.Services.VehicleControl.Interface;

public interface IVehicleControlServices
{
	Task<IEnumerable<VehicleControlDto>> GetAll(int page, int size, string search);
	Task<VehicleControlDto?> GetById(Guid id);
	Task<VehicleControlDto> Create(VehicleControlCreateDto control);
	Task<VehicleControlDto?> Update(VehicleControlUpdateDto control);
	Task Delete(Guid id);
	Task<int> TotalVehicleControl();
}
