using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ParkingLotApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ParkingOrdersController : ControllerBase
    {

        [HttpPost]
        public Task CreateNewParkingOrder()
        {
            // TODO
            return null;
        }

    }
}
