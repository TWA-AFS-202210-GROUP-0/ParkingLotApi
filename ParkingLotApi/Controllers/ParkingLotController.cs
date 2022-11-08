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
        private readonly ParkingOrderService parkingOrderService;

        public ParkingLotsController(ParkingLotService parkingLotService, ParkingOrderService parkingOrderService)
        {
            this.parkingLotService = parkingLotService;
            this.parkingOrderService = parkingOrderService;
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

        [HttpPost("{id}/orders")]
        public async Task<ActionResult<ParkingOrderDto>> CrateParkingLotOrderById(
            [FromRoute] int id, [FromBody] ParkingOrderDto parkingOrderDto)
        {
            var parkingOrder = await this.parkingOrderService.AddNewOrder(id, parkingOrderDto);
            return parkingOrder != null
                ? Created($"ParkingLots/{id}/orders/{parkingOrder.Number}", parkingOrder)
                : BadRequest();
        }

        [HttpPut("{id}/orders/{orderNumber}")]
        public async Task<ActionResult<ParkingOrderDto>> UpdateParkingLotOrderById(
            [FromRoute] int id, [FromRoute] int orderNumber, [FromBody] ParkingOrderDto parkingOrderDto)
        {
            var parkingOrder = await parkingOrderService.CloseOrder(id, orderNumber, parkingOrderDto);
            if (parkingOrder != null)
            {
                return Ok(parkingOrder);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ParkingLotDto>> GetById(int id)
        {
            var parkingLot = await parkingLotService.GetById(id);
            return parkingLot != null ? Ok(parkingLot) : NotFound();
        }

      // [HttpGet("{parkingLotid}/orders/{id}")]
      // public async Task<ActionResult<ParkingLotDto>> GetOrderById(int parkingLotid, int id)
      // {
      //     var parkingLot = await parkingLotService.GetById(id);
      //     return parkingLot != null ? Ok(parkingLot) : NotFound();
      // }

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