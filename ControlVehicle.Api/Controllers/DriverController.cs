using ControlVehicle.Api.Services.Driver.Interface;
using ControlVehicle.Dto.Driver;
using Microsoft.AspNetCore.Mvc;

namespace ControlVehicle.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DriverController(IDriverServices driverServices) : ControllerBase
{
    private readonly IDriverServices _driverServices = driverServices;
    [HttpGet]
    public async Task<ActionResult<IEnumerable<DriverDto>>> GetAll(int page = 1, int size = 10, string search = "")
    {
        var driverList = await _driverServices.GetAll(page, size, search);
        decimal totalData = await _driverServices.TotalDriver();
        decimal totalPage = (totalData / size) <= 0 ? 1 : Math.Ceiling(totalData / size);

        if (size == 1)
        {
            totalPage = totalData;
        }

        if (!driverList.Any())
        {
            return NotFound();
        }

        return Ok(new
        {
            totalData,
            page,
            totalPage,
            size,
            driverList
        });

    }
    [HttpGet("{cnh}", Name = "GetDriver")]
    public async Task<ActionResult<DriverDto>> GetByCnh(string cnh)
    {
        var driver = await _driverServices.GetByCnh(cnh);

        if (driver is not null)
        {
            return Ok(driver);
        }
        return NotFound();

    }
    [HttpPost]
    public async Task<ActionResult<DriverDto>> Post([FromBody] DriverDto driver)
    {
        if (driver is not null)
        {
            await _driverServices.Create(driver);
            return new CreatedAtRouteResult("GetDriver", new { cnh = driver.CNH }, driver);
        }
        return BadRequest();
    }

    [HttpPut("{id:Guid}")]
    public async Task<ActionResult<DriverDto>> Put(Guid id, [FromBody] DriverDto driver)
    {
        if (id != driver.Id)
        {
            return BadRequest();
        }
        if (driver is null)
        {
            return BadRequest();
        }

        await _driverServices.Update(driver);
        return Ok(driver);
    }
    [HttpDelete("{cnh}")]
    public async Task<ActionResult<DriverDto>> Delete(string cnh)
    {
        var driver = await _driverServices.GetByCnh(cnh);
        if (driver is null)
        {
            return NotFound();
        }
        if (driver.CNH == "")
        {
            return NotFound();
        }
        await _driverServices.Delete(cnh);
        return Ok(driver);
    }
}
