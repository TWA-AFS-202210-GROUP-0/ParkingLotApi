namespace ParkingLotApi.Controllers;

using Microsoft.AspNetCore.Mvc;
using ParkingLotApi.Services;
using System;
using System.Collections.Generic;
using System.Net;
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

    [HttpGet]
    public Task<List<ParkingLotDTO>> GetAll()
    {
        return parkingLotService.GetAllParkingLots();
    }

    [HttpPost]
    public async Task<ActionResult<int>> AddNew(ParkingLotDTO parkingLotDTO)
    {
        try
        {
            var id = await parkingLotService.AddNewParkingLot(parkingLotDTO);
            if (id == -1) {
                return StatusCode((int)HttpStatusCode.Forbidden, "Name or capacity is not acceptable.");
            }

            return id;
        }catch (Exception e)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete([FromRoute] int id)
    {
        await parkingLotService.DeleteParkingLot(id);

        return Ok();
    }
}