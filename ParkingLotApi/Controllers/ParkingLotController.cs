namespace ParkingLotApi.Controllers;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api")]
public class ParkingLotController : ControllerBase
{
    [HttpGet]
    public string Get()
    {
        return "Hello World";
    }
}