using System.Threading.Tasks;
using ParkingLotApi.Dto;
using ParkingLotApi.Repository;

namespace ParkingLotApi.Services
{
    public class ParkingOrderService : IParkingOrderService
    {
        private readonly ParkingDbContext _parkingDbContext;

        public ParkingOrderService(ParkingDbContext parkingDbParkingDbContext)
        {
            this._parkingDbContext = parkingDbParkingDbContext;
        }

        public async Task<ParkingOrderDto> CreateOrder(int parkingLotId, ParkingOrderDto parkingOrderDto)
        {
            
        }

        public async Task<ParkingOrderDto> CloseOrder(int parkingLotId, ParkingOrderDto parkingOrderDto)
        {
            return new ParkingOrderDto();
        }
    }
}
