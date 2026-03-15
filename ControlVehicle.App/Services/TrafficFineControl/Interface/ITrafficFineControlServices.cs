using ControlVehicle.Models.Dtos;

namespace ControlVehicle.App.Services.TrafficFineControl.Interface;

public interface ITrafficFineControlServices
{
    Task<IEnumerable<TrafficFineControlDto>> GetAll(int page, int size, string search);
    Task<TrafficFineControlDto?> GetById(Guid id);
    Task<Guid> Create(TrafficFineControlDto control);
    Task Update(TrafficFineControlDto control);
    Task Delete(Guid id);
    Task<int> TotalTrafficFineControl();
}
