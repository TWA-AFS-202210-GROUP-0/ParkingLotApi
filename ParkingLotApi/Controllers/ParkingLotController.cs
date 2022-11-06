using Microsoft.AspNetCore.Server.IIS.Core;
using ParkingLotApi.Dtos;
using ParkingLotApi.Services;

namespace ParkingLotApi.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

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

        [HttpPut("{id}")]
        public async Task<ActionResult<ParkingLotDto>> UpdateParkingLotCapacityById(
            [FromRoute] int id, [FromBody] ParkingLotDto parkingLotDto)
        {
            var parkingLot = await parkingLotService.UpdateById(id, parkingLotDto);
            return parkingLot != null ? parkingLot : NotFound();
        }

        [HttpPut("{id}/orders")]
        public async Task<ActionResult<ParkingLotDto>> UpdateParkingLotOrderById(
            [FromRoute] int id, [FromBody] ParkingLotDto parkingLotDto)
        {
            var parkingLot = await parkingLotService.UpdateOrderById(id, parkingLotDto);
            if (parkingLot != null)
            {
                if (parkingLot.Availibility > 0)
                {
                    return parkingLot;
                }
                else
                {
                    return BadRequest();
                }
            }
            else
            {
                return NotFound();
            }
        }

       // [HttpGet("{id}/orders")]
       // public async Task<ActionResult<ParkingLotDto>> CreateParkingLotOrderById(
       //     [FromRoute] int id, [FromBody] ParkingLotDto parkingLotDto)
       // {
       //     var parkingLot = await parkingLotService.UpdateOrderById(id, parkingLotDto);
       //     return parkingLot != null ? parkingLot : NotFound();
       // }

        [HttpGet("{id}")]
        public async Task<ActionResult<ParkingLotDto>> GetById(int id)
        {
            var parkingLot = await parkingLotService.GetById(id);
            return parkingLot != null ? Ok(parkingLot) : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await parkingLotService.DeleteParkingLot(id);
            return NoContent();
        }

        [HttpGet]
        public ActionResult<List<ParkingLotDto>> GetSeveralParkingLots([FromQuery] int? pageIndex)
        {
            return this.parkingLotService.GetParkingLots(pageIndex);
        }
    }
}