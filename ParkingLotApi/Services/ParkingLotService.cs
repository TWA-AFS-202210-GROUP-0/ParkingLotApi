using ParkingLotApi.Repository;

namespace ParkingLotApi.Services
{
    public class ParkingLotService
    {
        private readonly ParkingLotContext parkingLotDbcontext;

        public ParkingLotService(ParkingLotContext parkingLotDbcontext)
        {
            this.parkingLotDbcontext = parkingLotDbcontext;
        }
    }
}
