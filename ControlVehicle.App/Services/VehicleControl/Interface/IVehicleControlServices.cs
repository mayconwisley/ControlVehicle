using ControlVehicle.Models.Dtos;

namespace ControlVehicle.App.Services.VehicleControl.Interface;

public interface IVehicleControlServices
{
	Task<IEnumerable<VehicleControlDto>> GetAll(int page, int size, string search);
	Task<VehicleControlDto?> GetById(Guid id);
	Task<Guid> Create(VehicleControlDto control);
	Task Update(VehicleControlDto control);
	Task Delete(Guid id);
	Task<int> TotalVehicleControl();
}
