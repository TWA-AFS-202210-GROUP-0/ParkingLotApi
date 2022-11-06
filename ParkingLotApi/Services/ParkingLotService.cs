using ParkingLotApi.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkingLotApi.Services
{
    public class ParkingLotService
    {
        private readonly ParkingLotContext parkingLotContext;

        public ParkingLotService(ParkingLotContext parkingLotContext)
        {
            this.parkingLotContext = parkingLotContext;
        }

        public async Task<List<ParkingLotDTO>> GetAllParkingLots()
        {
            return parkingLotContext.ParkingLots.Select(p => new ParkingLotDTO(p)).ToList();
        }

        public async Task<List<ParkingLotDTO>> GetAllParkingLotsByPage(int pageNumber, int pageSize=15)
        {
            var startIndex = pageSize * (pageNumber - 1);
            var pageEnd = startIndex + pageSize;
            var total = parkingLotContext.ParkingLots.Count();
            var endIndex = pageEnd > total ? total : pageEnd;
            return parkingLotContext.ParkingLots
                .OrderBy(_ => _.Id)
                .Select(p => new ParkingLotDTO(p))
                .ToList()
                .GetRange(startIndex, endIndex - startIndex);
        }

        public ParkingLotDTO GetParkingLot(int id)
        {
            var parkingLot = parkingLotContext.ParkingLots.FirstOrDefault(p => p.Id == id);
            return parkingLot != null ? new ParkingLotDTO(parkingLot) : null;
        }

        public async Task<int> AddNewParkingLot(ParkingLotDTO parkingLotDTO)
        {
            ParkingLotEntity parkingLotEntity = parkingLotDTO.ToEntity();

            if (IsParkingLotNameExisted(parkingLotEntity)) { return -1; }
            if (!IsCapacityValid(parkingLotEntity)) { return -1; }

            this.parkingLotContext.ParkingLots.Add(parkingLotEntity);
            await this.parkingLotContext.SaveChangesAsync();

            return parkingLotEntity.Id;
        }

        public async Task DeleteParkingLot(int id)
        {
            var parkingLot = parkingLotContext.ParkingLots
                .FirstOrDefault(_ => _.Id == id);
            parkingLotContext.ParkingLots.RemoveRange(parkingLot);
            await parkingLotContext.SaveChangesAsync();
        }

        public async Task<ParkingLotDTO> UpdateParkingLotCapacity(int id, int capacity)
        {
            var parkingLot = parkingLotContext.ParkingLots
                .FirstOrDefault(_ => _.Id == id);
            if (parkingLot == null) { return null; }
            parkingLot.Capacity = capacity;
            parkingLotContext.ParkingLots.Update(parkingLot);
            await parkingLotContext.SaveChangesAsync();
            return new ParkingLotDTO(parkingLot);
        }

        private bool IsParkingLotNameExisted(ParkingLotEntity parkingLot)
        {
            var parkingLotInSystem = this.parkingLotContext.ParkingLots.FirstOrDefault(p => p.Name.Equals(parkingLot.Name));
            return parkingLotInSystem != null;
        }

        private bool IsCapacityValid(ParkingLotEntity parkingLot)
        {
            return parkingLot.Capacity >= 0;
        }

        public async Task<OrderDTO> AddCarInParkingLot(int id, string car)
        {
            var parkingLot = GetParkingLot(id);
            if (parkingLot == null) { return null;}

            if( IsAtCapacity(id) ) { throw new Exception("The parking lot is full"); }

            var orderEntity = new OrderEntity
            {
                OrderNumber = Guid.NewGuid().ToString(),
                ParkingLotName = parkingLot.Name,
                PlateNumber = car,
                CreationTime = DateTime.Now,
                Status = true,
            };
            parkingLotContext.Orders.Add(orderEntity);
            await parkingLotContext.SaveChangesAsync();
            return new OrderDTO(orderEntity);
        }

        private bool IsAtCapacity(int id)
        {
            var parkingLot = GetParkingLot(id);
            return parkingLotContext.Orders
                .Where(o => o.Id == id)
                .Where(o => o.Status).Count() >= parkingLot.Capacity;
        }

        public async Task<OrderDTO> CLoseParkingCarOrder(OrderDTO order)
        {
            var orderEntity = order.ToEntity();
            orderEntity.ClosedTime = DateTime.Now;
            orderEntity.Status = false;
            parkingLotContext.Orders.Update(orderEntity);
            await parkingLotContext.SaveChangesAsync();
            return new OrderDTO(orderEntity);
        }
    }
}
