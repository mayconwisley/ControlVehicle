using ControlVehicle.Api.Services.Vehicle.Interface;
using ControlVehicle.Dto.Vehicle;
using Microsoft.AspNetCore.Mvc;

namespace ControlVehicle.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class VehicleController(IVehicleServices vehicleServices) : ControllerBase
{
    private readonly IVehicleServices _vehicleServices = vehicleServices;
    [HttpGet]
    public async Task<ActionResult<IEnumerable<VehicleDto>>> GetAll(int page = 1, int size = 10, string search = "")
    {
        var vehicleList = await _vehicleServices.GetAll(page, size, search);
        decimal totalData = await _vehicleServices.TotalVehicle();
        decimal totalPage = (totalData / size) <= 0 ? 1 : Math.Ceiling(totalData / size);

        if (size == 1)
        {
            totalPage = totalData;
        }

        if (!vehicleList.Any())
        {
            return NotFound();
        }

        return Ok(new
        {
            totalData,
            page,
            totalPage,
            size,
            vehicleList
        });

    }
    [HttpGet("Plate/{plate}", Name = "GetVehicle")]
    public async Task<ActionResult<VehicleDto>> GetByPlate(string plate)
    {
        var vehicle = await _vehicleServices.GetByPlate(plate);

        if (vehicle is not null)
        {
            return Ok(vehicle);
        }
        return NotFound();

    }
    [HttpGet("Renavam/{renavam}")]
    public async Task<ActionResult<VehicleDto>> GetByRenavam(string renavam)
    {
        var vehicle = await _vehicleServices.GetByRenavam(renavam);

        if (vehicle is not null)
        {
            return Ok(vehicle);
        }
        return NotFound();

    }
    [HttpPost]
    public async Task<ActionResult<VehicleDto>> Post([FromBody] VehicleDto vehicle)
    {
        if (vehicle is not null)
        {
            await _vehicleServices.Create(vehicle);
            return new CreatedAtRouteResult("GetVehicle", new { renavam = vehicle.Renavam }, vehicle);
        }
        return BadRequest();
    }

    [HttpPut("{id:Guid}")]
    public async Task<ActionResult<VehicleDto>> Put(Guid id, [FromBody] VehicleDto vehicle)
    {
        if (id != vehicle.Id)
        {
            return BadRequest();
        }
        if (vehicle is null)
        {
            return BadRequest();
        }

        await _vehicleServices.Update(vehicle);
        return Ok(vehicle);
    }
    [HttpDelete("{renavam}")]
    public async Task<ActionResult<VehicleDto>> Delete(string renavam)
    {
        var vehicle = await _vehicleServices.GetByRenavam(renavam);
        if (vehicle is null)
        {
            return NotFound();
        }
        if (vehicle.Renavam == "")
        {
            return NotFound();
        }
        await _vehicleServices.Delete(renavam);
        return Ok(vehicle);
    }
}
