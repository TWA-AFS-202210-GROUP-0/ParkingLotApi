using Microsoft.AspNetCore.Mvc;
using ParkingLotApi.Dtos;
using ParkingLotApi.Exceptions;
using ParkingLotApi.Services;
using System.Threading.Tasks;

namespace ParkingLotApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ParkingOrdersController : Controller
    {

        private ParkingOrderService parkingOrderService;

        public ParkingOrdersController(ParkingOrderService parkingOrderService)
        {
            this.parkingOrderService = parkingOrderService;
        }

        [HttpPost]
        public async Task<ActionResult<ParkingOrderDto>> Add([FromBody] ParkingOrderDto parkingOrderDto)
        {
            try
            {
                var id = await this.parkingOrderService.AddParkingOrder(parkingOrderDto);

                //return CreatedAtAction(nameof(AddParkingLot), new { id = id }, parkingLotDto);
                return Created($"/ParkingOrders/{id}", parkingOrderDto);
            }
            catch(NoSpaceException e)
            {
                return BadRequest(e.Message);
            }
            
        }
    }
}
