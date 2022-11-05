namespace ParkingLotApi.Controllers;

using Microsoft.AspNetCore.Mvc;
using ParkingLotApi.Dtos;
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
    public async Task<ActionResult<ParkingLotDto>> AddParkingLot(ParkingLotDto parkingLotDto)
    {
        var id = await this.parkingLotService.AddParkingLot(parkingLotDto);

        //return CreatedAtAction(nameof(AddParkingLot), new { id = id }, parkingLotDto);
        return Created($"/ParkingLots/{id}", parkingLotDto);
    }
}