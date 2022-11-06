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
            var id = parkingLotService.AddParkingLot(parkingLotDto);
            return new CreatedResult($"parkinglots/{id}", parkingLotDto);
        }
        catch (DuplicateNameException e)
        {
            return new ConflictResult();
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteParkingLot([FromRoute]int id,[FromQuery] string name)
    {
        try
        {
            await parkingLotService.DeleteParkingLot(id, name);
            return new OkResult();
        }
        catch (NullReferenceException e)
        {
            return new NotFoundResult();
        }
    }

    [HttpGet("{pageIndex}")]
    public async Task<ActionResult<List<ParkingLotDto>>> Get15ParkingLotsOnePage([FromRoute] int pageIndex)
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
            return new NotFoundResult();
        }
    }



}