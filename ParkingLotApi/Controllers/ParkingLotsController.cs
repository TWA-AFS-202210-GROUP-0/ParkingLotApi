namespace ParkingLotApi.Controllers;

using Microsoft.AspNetCore.Mvc;
using ParkingLotApi.Dtos;
using ParkingLotApi.Exceptions;
using ParkingLotApi.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

[ApiController]
[Route("[controller]")]
public class ParkingLotsController : ControllerBase
{
    private ParkingLotService parkingLotService;

    public ParkingLotsController(ParkingLotService parkingLotService)
    {
        this.parkingLotService = parkingLotService;
    }

    [HttpPost]
    public async Task<ActionResult<ParkingLotDto>> Add([FromBody] ParkingLotDto parkingLotDto)
    {
        try
        {
            var id = await this.parkingLotService.AddParkingLot(parkingLotDto);

            //return CreatedAtAction(nameof(AddParkingLot), new { id = id }, parkingLotDto);
            return Created($"/ParkingLots/{id}", parkingLotDto);
        }
        catch (WrongNameException e)
        {
            return Conflict(e.Message);
        }
        catch (WrongCapacityException e)
        {
            return BadRequest(e.Message);
        }
        
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete([FromRoute] int id)
    {
        await parkingLotService.DeleteParkingLot(id);
        return this.NoContent();
    }

    [HttpGet("page/{pageIndex}")]
    public async Task<ActionResult<IEnumerable<ParkingLotNoLocationDto>>> GetByPage([FromRoute] int pageIndex)
    {
        var parkingLotNoLocationDtos = await parkingLotService.GetParkingLotByPage(pageIndex);
        return Ok(parkingLotNoLocationDtos);
        
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ParkingLotDto>> GetById([FromRoute] int id)
    {
        var parkingLotDto = await parkingLotService.GetParkingLotById(id);
        return Ok(parkingLotDto);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ParkingLotDto>> ModifyCapaicty([FromBody] ParkingLotDto parkingLotDto)
    {
        var updateParkingLotDto = await parkingLotService.ModifyParkingLotCapacity(parkingLotDto);
        return Ok(updateParkingLotDto);
    }
}