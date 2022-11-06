using ParkingLotApi.Dtos;
using ParkingLotApi.Models;
using ParkingLotApi.Repository;
using System;
using System.Threading.Tasks;

namespace ParkingLotApi.Services
{
    public class ParkingOrderService
    {
        private readonly ParkingLotContext parkingLotContext;
        public ParkingOrderService(ParkingLotContext parkingLotContext)
        {
            this.parkingLotContext = parkingLotContext;
        }

        public async Task<int>  AddParkingOrder(ParkingOrderDto parkingOrderDto)
        {
            ParkingOrderEntity parkingOrderEntity = parkingOrderDto.ToEntity();
            await parkingLotContext.ParkingOrders.AddAsync(parkingOrderEntity);
            await parkingLotContext.SaveChangesAsync();
            return parkingOrderEntity.Id;
        }
    }
}
