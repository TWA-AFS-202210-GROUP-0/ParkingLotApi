using System;
using System.Data;
using System.Threading.Tasks;
using ParkingLotApi.Dto;
using ParkingLotApi.Services;

namespace ParkingLotApi.Controllers;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("parkinglots")]
public class ParkingLotController : ControllerBase
{
    private readonly IParkingLotService parkingLotService;

    public ParkingLotController(IParkingLotService parkingLotService)
    {
        this.parkingLotService = parkingLotService;
    }

    [HttpPost]
    public async Task<ActionResult<ParkingLotDto>> CreateParkingLot([FromBody] ParkingLotDto parkingLotDto)
    {
        try
        {
            var id = parkingLotService.AddParkingLot(parkingLotDto);
            return new CreatedResult($"parkinglots/{id}", parkingLotDto);
        }
        catch (DuplicateNameException e)
        {
            return Conflict(e.Message);
        }
    }
}