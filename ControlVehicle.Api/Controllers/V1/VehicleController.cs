using Asp.Versioning;
using ControlVehicle.App.Services.Vehicle.Interface;
using ControlVehicle.Models.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace ControlVehicle.Api.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class VehicleController(IVehicleServices vehicleServices) : ControllerBase
{
    private readonly IVehicleServices _vehicleServices = vehicleServices;

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<VehicleDto>>> GetAll(int page = 1, int size = 5, string search = "")
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

    [HttpGet("Plate/{plate}", Name = "GetVehicleV1")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<VehicleDto>> GetByPlate(string plate)
    {
        var vehicle = await _vehicleServices.GetByPlate(plate);
        if (vehicle is null)
        {
            return NotFound();
        }

        return Ok(vehicle);
    }

    [HttpGet("Renavam/{renavam}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<VehicleDto>> GetByRenavam(string renavam)
    {
        var vehicle = await _vehicleServices.GetByRenavam(renavam);
        if (vehicle is null)
        {
            return NotFound();
        }

        return Ok(vehicle);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<VehicleDto>> Post([FromBody] VehicleDto vehicle)
    {
        if (vehicle is null)
        {
            return BadRequest();
        }

        await _vehicleServices.Create(vehicle);
        return new CreatedAtRouteResult("GetVehicleV1", new { version = "1", plate = vehicle.LicensePlate }, vehicle);
    }

    [HttpPut("{id:Guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<VehicleDto>> Put(Guid id, [FromBody] VehicleDto vehicle)
    {
        if (vehicle is null || id != vehicle.Id)
        {
            return BadRequest();
        }

        await _vehicleServices.Update(vehicle);
        return Ok(vehicle);
    }

    [HttpDelete("{renavam}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<VehicleDto>> Delete(string renavam)
    {
        var vehicle = await _vehicleServices.GetByRenavam(renavam);
        if (vehicle is null)
        {
            return NotFound();
        }

        await _vehicleServices.Delete(renavam);
        return Ok(vehicle);
    }
}
