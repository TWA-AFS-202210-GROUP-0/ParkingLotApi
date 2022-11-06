using Microsoft.AspNetCore.Server.IIS.Core;
using ParkingLotApi.Dtos;
using ParkingLotApi.Services;

namespace ParkingLotApi.Controllers;

using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[ApiController]
[Route("[controller]")]
public class ParkingLotsController : ControllerBase
{
    private readonly ParkingLotService parkingLotService;

    public ParkingLotsController(ParkingLotService parkingLotService)
    {
        this.parkingLotService = parkingLotService;
    }

    [HttpPost]
    public async Task<ActionResult<ParkingLotDto>> AddParkingLot([FromBody] ParkingLotDto parkingLotDto)
    {
        var parkinglot = await this.parkingLotService.AddNewParkingLot(parkingLotDto);
        return parkinglot != null
            ? CreatedAtAction(nameof(GetById), new { id = parkinglot.Id }, parkingLotDto)
            : BadRequest();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ParkingLotDto>> GetById(int id)
    {
        var parkingLot = parkingLotService.GetById(id);
        return Ok(parkingLot);
    }

}