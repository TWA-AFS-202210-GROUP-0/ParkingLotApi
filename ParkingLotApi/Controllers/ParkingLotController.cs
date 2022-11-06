using Microsoft.AspNetCore.Mvc;
using ParkingLotApi.Dto;
using ParkingLotApi.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ParkingLotApi.Controllers
{
    [ApiController]
    [Route("companies")]
    public class ParkingLotController : Controller
    {
        private readonly parkingLotService parkingLotService;

        public ParkingLotController(parkingLotService parkingLotService)
        {
            this.parkingLotService = parkingLotService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ParkingLotDto>>> List([FromQuery] string pageIndex)
        {
            var ParkingLotDtos = await this.parkingLotService.GetAll();

            try
            {
                int pageSizeInt = 15;
                int pageIndexInt = Convert.ToInt32(pageIndex);
                return ParkingLotDtos.Skip(pageSizeInt * (pageIndexInt - 1)).Take(pageSizeInt).ToList();
            }
            catch (Exception e)
            {
                return Ok(ParkingLotDtos);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ParkingLotDto>> GetById(int id)
        {
            var ParkingLotDto = await this.parkingLotService.GetById(id);
            return Ok(ParkingLotDto);
        }

        [HttpPost]
        public async Task<ActionResult<ParkingLotDto>> Add(ParkingLotDto ParkingLotDto)
        {
            try
            {
                var id = await this.parkingLotService.AddOrUpdateCompany(ParkingLotDto, true);
                return CreatedAtAction(nameof(GetById), new { id = id }, ParkingLotDto);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult<ParkingLotDto>> Update(ParkingLotDto ParkingLotDto)
        {
            try
            {
                var id = await this.parkingLotService.AddOrUpdateCompany(ParkingLotDto, false);

                return CreatedAtAction(nameof(GetById), new { id = id }, ParkingLotDto);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await parkingLotService.DeleteCompany(id);
            return this.NoContent();
        }

    }
}