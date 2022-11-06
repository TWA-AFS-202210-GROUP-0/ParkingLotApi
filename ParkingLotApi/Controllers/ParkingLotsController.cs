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
    public Task<List<ParkingLotDTO>> GetAll([FromQuery] int? pageNumber)
    {
        if (pageNumber != null)
        {
            return parkingLotService.GetAllParkingLotsByPage((int)pageNumber);
        }

        return parkingLotService.GetAllParkingLots();
    }

    [HttpPost]
    public async Task<ActionResult<int>> AddNew(ParkingLotDTO parkingLotDTO)
    {
        try
        {
            var id = await parkingLotService.AddNewParkingLot(parkingLotDTO);
            if (id == -1)
            {
                return StatusCode((int)HttpStatusCode.Forbidden, "Name or capacity is not acceptable.");
            }

            return id;
        }
        catch (Exception e)
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

    [HttpGet("{id}")]
    public async Task<ActionResult<ParkingLotDTO>> GetById([FromRoute] int id)
    {
        var parkingLotDTO = parkingLotService.GetParkingLot(id);
        return Ok(parkingLotDTO);
    }

    [HttpPatch("{id}")]
    public async Task<ActionResult<ParkingLotDTO>> GetById([FromRoute] int id, [FromBody] int capacity)
    {
        var parkingLotDTO = await parkingLotService.UpdateParkingLotCapacity(id, capacity);
        if (parkingLotDTO == null) { return NotFound(); }
        return Ok(parkingLotDTO);
    }

    [HttpPost("{id}/orders")]
    public async Task<ActionResult<OrderDTO>> GetById([FromRoute] int id, [FromBody] string carPlate)
    {
        var orderDTO = await parkingLotService.AddCarInParkingLot(id, carPlate);
        if (orderDTO == null) { return NotFound($"Not found parking lot with id {id}"); }
        return Ok(orderDTO);
    }
}