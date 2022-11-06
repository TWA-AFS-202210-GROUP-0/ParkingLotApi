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
    public async Task<ActionResult<ParkingLotDto>> AddParkingLot(ParkingLotDto parkingLotDto)
    {
        int id = await this.parkingLotService.AddNewParkingLot(parkingLotDto);
        return CreatedAtAction(nameof(GetById), new { id = id }, parkingLotDto);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ParkingLotDto>> GetById(int id)
    {
        var parkingLot = parkingLotService.GetById(id);
        return Ok(parkingLot);
    }

}