using Asp.Versioning;
using ControlVehicle.App.Services.Driver.Interface;
using ControlVehicle.Models.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ControlVehicle.Api.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class DriverController(IDriverServices driverServices) : ControllerBase
{
	private readonly IDriverServices _driverServices = driverServices;

	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<IEnumerable<DriverDto>>> GetAll(int page = 1, int size = 5, string search = "")
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

	[HttpGet("{cnh}", Name = "GetDriverV1")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<DriverDto>> GetByCnh(string cnh)
	{
		var driver = await _driverServices.GetByCnh(cnh);
		if (driver is null)
		{
			return NotFound();
		}

		return Ok(driver);
	}

	[HttpPost]
	[ProducesResponseType(StatusCodes.Status201Created)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<DriverDto>> Post([FromBody] DriverDto driver)
	{
		if (driver is null)
		{
			return BadRequest();
		}

		await _driverServices.Create(driver);
		return new CreatedAtRouteResult("GetDriverV1", new { version = "1", cnh = driver.Cnh }, driver);
	}

	[HttpPut("{id:Guid}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<DriverDto>> Put(Guid id, [FromBody] DriverDto driver)
	{
		if (driver is null || id != driver.Id)
		{
			return BadRequest();
		}

		await _driverServices.Update(driver);
		return Ok(driver);
	}

	[HttpDelete("{cnh}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<DriverDto>> Delete(string cnh)
	{
		var driver = await _driverServices.GetByCnh(cnh);
		if (driver is null)
		{
			return NotFound();
		}

		await _driverServices.Delete(cnh);
		return Ok(driver);
	}
}
