using Asp.Versioning;
using ControlVehicle.App.Services.FuelControl.Interface;
using ControlVehicle.Models.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ControlVehicle.Api.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class FuelControlController(IFuelControlServices controlServices) : ControllerBase
{
	private readonly IFuelControlServices _controlServices = controlServices;

	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<IEnumerable<FuelControlDto>>> GetAll(int page = 1, int size = 5, string search = "")
	{
		var controlList = await _controlServices.GetAll(page, size, search);
		decimal totalData = await _controlServices.TotalFuelControl();
		decimal totalPage = (totalData / size) <= 0 ? 1 : Math.Ceiling(totalData / size);

		if (size == 1)
		{
			totalPage = totalData;
		}

		if (!controlList.Any())
		{
			return NotFound();
		}

		return Ok(new
		{
			totalData,
			page,
			totalPage,
			size,
			controlList
		});
	}

	[HttpGet("{id:Guid}", Name = "GetFuelControlV1")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<FuelControlDto>> GetById(Guid id)
	{
		var control = await _controlServices.GetById(id);
		if (control is null)
		{
			return NotFound();
		}

		return Ok(control);
	}

	[HttpPost]
	[ProducesResponseType(StatusCodes.Status201Created)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<FuelControlDto>> Post([FromBody] FuelControlDto control)
	{
		if (control is null)
		{
			return BadRequest();
		}

		var id = await _controlServices.Create(control);
		return new CreatedAtRouteResult("GetFuelControlV1", new { version = "1", id }, control with { Id = id });
	}

	[HttpPut("{id:Guid}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<FuelControlDto>> Put(Guid id, [FromBody] FuelControlDto control)
	{
		if (control is null || id != control.Id)
		{
			return BadRequest();
		}

		await _controlServices.Update(control);
		return Ok(control);
	}

	[HttpDelete("{id:Guid}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<FuelControlDto>> Delete(Guid id)
	{
		var control = await _controlServices.GetById(id);
		if (control is null)
		{
			return NotFound();
		}

		await _controlServices.Delete(id);
		return Ok(control);
	}
}
