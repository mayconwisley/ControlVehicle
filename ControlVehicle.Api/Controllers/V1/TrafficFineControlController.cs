using Asp.Versioning;
using ControlVehicle.App.Services.TrafficFineControl.Interface;
using ControlVehicle.Models.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace ControlVehicle.Api.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class TrafficFineControlController(ITrafficFineControlServices controlServices) : ControllerBase
{
    private readonly ITrafficFineControlServices _controlServices = controlServices;

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<TrafficFineControlDto>>> GetAll(int page = 1, int size = 5, string search = "")
    {
        var controlList = await _controlServices.GetAll(page, size, search);
        decimal totalData = await _controlServices.TotalTrafficFineControl();
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

    [HttpGet("{id:Guid}", Name = "GetTrafficFineControlV1")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TrafficFineControlDto>> GetById(Guid id)
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
    public async Task<ActionResult<TrafficFineControlDto>> Post([FromBody] TrafficFineControlCreateDto control)
    {
        if (control is null)
        {
            return BadRequest();
        }

        try
        {
            var createdControl = await _controlServices.Create(control);
            return new CreatedAtRouteResult("GetTrafficFineControlV1", new { version = "1", id = createdControl.Id }, createdControl);
        }
        catch (InvalidOperationException ex)
        {
            return Problem(statusCode: StatusCodes.Status400BadRequest, title: "Validacao de CNH", detail: ex.Message);
        }
    }

    [HttpPut("{id:Guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TrafficFineControlDto>> Put(Guid id, [FromBody] TrafficFineControlUpdateDto control)
    {
        if (control is null || id != control.Id)
        {
            return BadRequest();
        }

        try
        {
            var updatedControl = await _controlServices.Update(control);
            if (updatedControl is null)
            {
                return NotFound();
            }

            return Ok(updatedControl);
        }
        catch (InvalidOperationException ex)
        {
            return Problem(statusCode: StatusCodes.Status400BadRequest, title: "Validacao de CNH", detail: ex.Message);
        }
    }

    [HttpDelete("{id:Guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TrafficFineControlDto>> Delete(Guid id)
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
