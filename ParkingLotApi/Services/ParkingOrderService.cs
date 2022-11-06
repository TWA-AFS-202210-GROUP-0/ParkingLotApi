using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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
            var parkingLotEntity = await _parkingDbContext.ParkingLots.Include(e => e.ParkingOrderEntities).SingleOrDefaultAsync(e => e.Id == parkingLotId);
            if (parkingLotEntity == null)
            {
                throw new NullReferenceException("Parking lot not found");
            }

            if (parkingLotEntity.Capacity - parkingLotEntity.ParkingOrderEntities.Select(e => e.IsOpen == true).ToList().Count() < 1)
            {
                throw new NullReferenceException("The parking lot is full.");
            }

            var parkingOrderEntity = DtoConverter.ToEntity(parkingOrderDto);
            parkingOrderEntity.CreateTime = DateTime.Now;
            parkingOrderEntity.NameOfParkingLot = parkingLotEntity.Name;
            parkingLotEntity.ParkingOrderEntities.Add(parkingOrderEntity);
            await _parkingDbContext.SaveChangesAsync();
            return DtoConverter.ToDto(parkingOrderEntity);

        }

        public async Task<ParkingOrderDto> CloseOrder(int parkingLotId, ParkingOrderDto parkingOrderDto)
        {
            var parkingOrderById =
                await _parkingDbContext.ParkingOrders.SingleOrDefaultAsync(e => e.Id == parkingOrderDto.Id);
            if (parkingOrderById == null)
            {
                throw new NullReferenceException();
            }

            parkingOrderById.IsOpen = false;
            parkingOrderById.CloseTime = DateTime.Now;
            _parkingDbContext.SaveChangesAsync();
            return DtoConverter.ToDto(parkingOrderById);

        }
    }
}
