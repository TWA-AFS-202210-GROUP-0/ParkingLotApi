using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ParkingLotApi.Dtos;
using ParkingLotApi.Services;

namespace ParkingLotApi.Controllers
{
    [ApiController]
    [Route("parkingLots")]
    public class ParkingLotController : ControllerBase
    {
        private IParkingLotService _parkingLotService;

        public ParkingLotController(IParkingLotService parkingLotService)
        {
            _parkingLotService = parkingLotService;
        }

        [HttpGet]
        public async Task<List<ParkingLotDto>> List()
        {
            return await _parkingLotService.GetAll();
        }

        [HttpPost]
        public async Task<ActionResult<int>> AddParkingLot(ParkingLotDto parkingLotDto)
        {
            if (parkingLotDto.Capacity < 0)
            {
                return Accepted();
            }

            var id = await _parkingLotService.AddParkingLot(parkingLotDto);
            if (id == -1)
            {
                return Conflict();
            }

            return CreatedAtAction(nameof(GetParkingLotById), new { id = id }, id);
        }

        [HttpGet("{id}")]

        public async Task<ActionResult<ParkingLotDto>> GetParkingLotById([FromRoute]int id)
        {
            var parkingLotDt = await _parkingLotService.GetParkingLotById(id);
            if (parkingLotDt == null)
            {
                return NotFound();
            }

            return Ok(parkingLotDt);
        }
    }
}
