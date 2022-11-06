using ParkingLotApi.Dto;

namespace ParkingLotApi.Controllers;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("parkinglots")]
public class ParkingLotController : ControllerBase
{

    [HttpPost]
    public ActionResult<ParkingLotDto> ParkingLot([FromBody] ParkingLotDto parkingLotDto)
    {
        return new CreatedResult("/parkinglots/1", new ParkingLotDto() { });
    }
}