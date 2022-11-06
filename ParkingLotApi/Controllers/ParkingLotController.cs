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

        [HttpGet("{id}")]
        public async Task<ActionResult<ParkingLotDto>> GetById(int id)
        {
            var parkingLot = await parkingLotService.GetById(id);
            return Ok(parkingLot);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await parkingLotService.DeleteParkingLot(id);
            return this.NoContent();
        }

        [HttpGet]
        public ActionResult<List<ParkingLotDto>> GetSeveralParkingLots([FromQuery] int? pageIndex)
        {
            return this.parkingLotService.GetParkingLots(pageIndex);
        }


    }
}

