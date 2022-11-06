using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParkingLotApi.Dto;
using ParkingLotApi.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkingLotApi.Service
{
    public class parkingLotService
    {
        private readonly ParkingLotContext parkingLotContext;

        public parkingLotService(ParkingLotContext parkingLotContext)
        {
            this.parkingLotContext = parkingLotContext;
        }

        public async Task<List<ParkingLotDto>> GetAll()
        {
            var parkingLotEntity = this.parkingLotContext.ParkingLots
                .ToList();

            return parkingLotEntity.Select(c => new ParkingLotDto(c)).ToList();
        }

        public async Task<ParkingLot> GetEntityByName(string Name)
        {
            try
            {
                var findParkLoting = await parkingLotContext.ParkingLots
    .FirstOrDefaultAsync(c => c.Name.Equals(Name));
                return findParkLoting;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<ParkingLotDto> GetById(int id)
        {
            try
            {
                var findParkLoting = await parkingLotContext.ParkingLots.FirstOrDefaultAsync(c => c.Id == id);
                return new ParkingLotDto(findParkLoting);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<int> AddOrUpdateCompany(ParkingLotDto parkingLotDto, bool isAdd)
        {

            if (parkingLotDto.Capacity <= 0)
            {
                throw new Exception("capacity must grater than 0");
            }

            var entity = await this.GetEntityByName(parkingLotDto.Name);
            if (entity != null && isAdd)
            {
                throw new Exception("name deplicated");
            }

            if (entity == null)
            {
                ParkingLot parkingLotEntity = parkingLotDto.ToEntity();
                await this.parkingLotContext.ParkingLots.AddAsync(parkingLotEntity);
                await this.parkingLotContext.SaveChangesAsync();

                return parkingLotEntity.Id;
            }
            else
            {
                parkingLotContext.Entry(entity).CurrentValues.SetValues(parkingLotDto);
                await this.parkingLotContext.SaveChangesAsync();
                return entity.Id;
            }
        }

        public async Task DeleteCompany(int id)
        {
            var findParkLoting = await parkingLotContext.ParkingLots
                .FirstOrDefaultAsync(c => c.Id == id);
            parkingLotContext.ParkingLots.RemoveRange(findParkLoting);
            await this.parkingLotContext.SaveChangesAsync();
        }

        public async Task<ParkingLotDto> CreateParkingOrder(OrderDto dto)
        {
            dto.CreateTime = DateTime.Now;
            var parkingEntity = await this.GetEntityByName(dto.Name);

            if (parkingEntity.Capacity <= 0)
            {
                throw new Exception("The parking lot is full");
            }
            parkingEntity.Capacity = parkingEntity.Capacity - 1;

            Order order = dto.toEntity();
            await parkingLotContext.Orders.AddAsync(order);
            await this.parkingLotContext.SaveChangesAsync();

            var OrderId = order.Id;
            parkingEntity.OrderId = OrderId;
            var parkingDto = new ParkingLotDto(parkingEntity);
            parkingLotContext.Entry(parkingEntity).CurrentValues.SetValues(parkingDto);
            await this.parkingLotContext.SaveChangesAsync();
            return parkingDto;
        }
        public async Task<OrderDto> updateOrder(OrderDto dto)
        {
            var parkingEntity = await this.GetEntityByName(dto.Name);
            var order = await parkingLotContext.Orders.FirstOrDefaultAsync(o => o.Id == parkingEntity.OrderId);

            dto.CloseTime = DateTime.Now;

            parkingLotContext.Entry(order).CurrentValues.SetValues(dto);

            await this.parkingLotContext.SaveChangesAsync();
            return dto;
        }


    }
}
