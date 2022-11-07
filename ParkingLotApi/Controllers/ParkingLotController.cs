using System;
using System.Collections.Generic;
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
        if (parkingLotDto.Capacity < 0)
        {
            return BadRequest();
        }
        try
        {
            var id = await parkingLotService.AddParkingLot(parkingLotDto);
            return Created($"parkinglots/{id}", parkingLotDto);
        }
        catch (DuplicateNameException e)
        {
            return Conflict();
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteParkingLot([FromRoute]int id,[FromQuery] string name)
    {
        try
        {
            await parkingLotService.DeleteParkingLot(id, name);
            return Ok();
        }
        catch (NullReferenceException e)
        {
            return NotFound();
        }
    }

    [HttpGet]
    public async Task<ActionResult<List<ParkingLotDto>>> Get15ParkingLotsOnePage([FromQuery] int pageIndex)
    {
        if (pageIndex < 1)
        {
            return BadRequest();
        }
        try
        {
            var parkingLots = await parkingLotService.GetMultiParkingLots(15 * (pageIndex - 1), 15);
            return Ok(parkingLots);
        }
        catch (NullReferenceException e)
        {
            return NotFound();
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ParkingLotDto>> GetParkingLotsById([FromRoute] int id)
    {
        try
        {
            var parkingLot = await parkingLotService.GetParkingLotById(id);
            return Ok(parkingLot);
        }
        catch (NullReferenceException e)
        {
            return NotFound();
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ParkingLotDto>> PutParkingLot([FromRoute]int id, [FromBody] ParkingLotDto parkingLotDto)
    {
        try
        {
            var updatedParkingLotDto = await parkingLotService.UpdateParkingLot(id, parkingLotDto);
            return Ok(updatedParkingLotDto);
        }
        catch (NullReferenceException ex)
        {
            return NotFound();
        }
    }
}