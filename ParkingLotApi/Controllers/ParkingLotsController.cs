namespace ParkingLotApi.Controllers;

using Microsoft.AspNetCore.Mvc;
using ParkingLotApi.Dtos;
using ParkingLotApi.Exceptions;
using ParkingLotApi.Services;
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
    public async Task<ActionResult<ParkingLotDto>> AddParkingLot([FromBody] ParkingLotDto parkingLotDto)
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
}