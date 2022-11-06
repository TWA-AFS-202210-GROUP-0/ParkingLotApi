using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ParkingLotApi.Dto;
using ParkingLotApi.Services;

namespace ParkingLotApi.Controllers
{

    [ApiController]
    [Route("parkinglots/{parkingLotId}/orders")]
    public class ParkingOrderController : Controller
    {
        private readonly IParkingOrderService _parkingOrderService;

        public ParkingOrderController(IParkingOrderService parkingOrderService)
        {
            _parkingOrderService = parkingOrderService;
        }

        [HttpPost]
        public async Task<ActionResult<ParkingOrderDto>> PostOrder([FromRoute] int parkingLotId,
            [FromBody] ParkingOrderDto parkingOrderDto)
        {
            try
            {
                var createdOrder = await _parkingOrderService.CreateOrder(parkingLotId, parkingOrderDto);
                return new CreatedResult($"parkinglots/{parkingLotId}/orders/{createdOrder.Id}", createdOrder);
            }
            catch (NullReferenceException e)
            {
                return NotFound("The parking lot is full");
            }
        }

        [HttpPut]
        public async Task<ActionResult<ParkingOrderDto>> UpdateOrder([FromRoute] int parkingLotId, ParkingOrderDto parkingOrderDto)
        {
            try
            {
                var updatedParkingOrderDto = await _parkingOrderService.CloseOrder(parkingLotId, parkingOrderDto);
                return Ok(updatedParkingOrderDto);
            }
            catch (NullReferenceException e)
            {
                return NotFound();
            }
        }
    }
}
