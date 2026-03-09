using Asp.Versioning;
using ControlVehicle.App.Services.DriverCnh.Interface;
using ControlVehicle.Models.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace ControlVehicle.Api.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/cnh")]
public class CnhController(IDriverCnhServices driverCnhServices) : ControllerBase
{
	private readonly IDriverCnhServices _driverCnhServices = driverCnhServices;

	[HttpGet]
	public async Task<ActionResult<IEnumerable<DriverCnhDto>>> GetAll(int page = 1, int size = 10, string search = "")
	{
		var cnhList = await _driverCnhServices.GetAll(page, size, search);
		var totalData = await _driverCnhServices.Total();
		var totalPage = (decimal)Math.Ceiling(totalData / (double)size);

		return Ok(new
		{
			totalData,
			page,
			totalPage,
			size,
			cnhList
		});
	}

	[HttpGet("{number}", Name = "GetCnhV1")]
	public async Task<ActionResult<DriverCnhDto>> GetByNumber(string number)
	{
		var cnh = await _driverCnhServices.GetByCnh(number);
		if (cnh is null)
		{
			return NotFound();
		}

		return Ok(cnh);
	}

	[HttpPost]
	public async Task<ActionResult<DriverCnhDto>> Post([FromBody] DriverCnhDto cnhDto)
	{
		if (cnhDto is null)
		{
			return BadRequest();
		}

		await _driverCnhServices.Create(cnhDto);
		return new CreatedAtRouteResult("GetCnhV1", new { version = "1", number = cnhDto.Cnh.Number }, cnhDto);
	}

	[HttpPut("{id:Guid}")]
	public async Task<ActionResult<DriverCnhDto>> Put(Guid id, [FromBody] DriverCnhDto cnhDto)
	{
		if (cnhDto is null || id != cnhDto.Id)
		{
			return BadRequest();
		}

		await _driverCnhServices.Update(cnhDto);
		return Ok(cnhDto);
	}

	[HttpDelete("{number}")]
	public async Task<IActionResult> Delete(string number)
	{
		await _driverCnhServices.Delete(number);
		return NoContent();
	}
}
