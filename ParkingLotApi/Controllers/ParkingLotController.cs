using Microsoft.AspNetCore.Mvc;
using ParkingLotApi.Dto;
using ParkingLotApi.Repository;
using ParkingLotApi.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ParkingLotApi.Controllers
{
    [ApiController]
    [Route("parkinglot")]
    public class ParkingLotController : Controller
    {
        private readonly parkingLotService parkingLotService;

        public ParkingLotController(parkingLotService parkingLotService)
        {
            this.parkingLotService = parkingLotService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ParkingLotDto>>> List([FromQuery] string? pageIndex)
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

                return Ok(id);
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

        [HttpPost("{Name}/Order")]
        public async Task<ActionResult<ParkingLotDto>> CreateOrder(OrderDto orderDto)
        {
            try
            {
                var id = await this.parkingLotService.CreateParkingOrder(orderDto);
                return CreatedAtAction(nameof(GetById), new { id = id }, orderDto);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [HttpPut("{Name}/Order")]
        public async Task<ActionResult<ParkingLotDto>> UpdateOrder(OrderDto orderDto)
        {
            try
            {
                var dto = await this.parkingLotService.updateOrder(orderDto);

                return Ok(dto);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}