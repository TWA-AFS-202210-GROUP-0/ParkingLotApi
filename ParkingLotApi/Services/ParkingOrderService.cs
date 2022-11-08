using System;
using System.Linq;
using ParkingLotApi.Repository;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ParkingLotApi.Dtos;
using ParkingLotApi.Models;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Collections.Generic;
using System.Web.Http;

namespace ParkingLotApi.Services
{
    public class ParkingOrderService
    {
        private readonly ParkingLotContext parkingLotDbcontext;

        public ParkingOrderService(ParkingLotContext parkingLotDbcontext)
        {
            this.parkingLotDbcontext = parkingLotDbcontext;
        }

        public async Task<ParkingOrderDto> AddNewOrder(int parkingLotId, ParkingOrderDto parkingOrderDto)
        {
            var parkingLot = await parkingLotDbcontext.ParkingLots.Include(_ => _.ParkingOrder).
                FirstOrDefaultAsync(
                parkinglot => parkinglot.Id == parkingLotId);
            var availability = parkingLot.Capacity -
                               parkingLot.ParkingOrder.Where(_ => _.OrderStatus == OrderStatus.OPEN).ToList().Count;
            if (availability <= 0)
            {
                throw new FullParkingLotException("The parking lot is full!");
            }

            if (parkingLot == null)
            {
                return null;
            }

            //1. convert dto to entity
            ParkingOrderEntity parkingOrderEntity = parkingOrderDto.ToEntity();
            parkingOrderEntity.CreateTime = DateTime.Now;
            parkingOrderEntity.NameOfParkingLot = parkingLot.Name;
            parkingOrderEntity.CarPlateNumber = parkingOrderDto.CarPlateNumber;
            //2. save entity to parking lot and db
            parkingLot.ParkingOrder.Add(parkingOrderEntity);
            await parkingLotDbcontext.Orders.AddAsync(parkingOrderEntity);
            parkingLotDbcontext.ParkingLots.Update(parkingLot);
            await parkingLotDbcontext.SaveChangesAsync();
            // 2+. set number as id
            parkingOrderEntity.Number = parkingOrderEntity.Id;
            // 3. return Order dto
            return new ParkingOrderDto(parkingOrderEntity);
        }

        public async Task<ParkingOrderDto> CloseOrder(int parkingLotId, int orderNumber, ParkingOrderDto parkingOrderDto)
        {
            var parkingLotEntity = await parkingLotDbcontext.ParkingLots.Include(_ => _.ParkingOrder)
                .FirstOrDefaultAsync(_ => _.Id == parkingLotId);
            var parkingOrder = parkingLotEntity.ParkingOrder.FirstOrDefault(_ => _.Id == orderNumber);
            if (parkingOrder == null)
            {
                throw new NullReferenceException();
            }

            parkingOrder.CloseTime = DateTime.Now;
            parkingOrder.OrderStatus = OrderStatus.CLOSE;
            parkingLotDbcontext.Orders.Update(parkingOrder);
            parkingLotDbcontext.ParkingLots.Update(parkingLotEntity);
            await parkingLotDbcontext.SaveChangesAsync();
            return new ParkingOrderDto(parkingOrder);
        }

        public async Task<ParkingOrderDto> GetOrderById(int id)
        {
            var parkingOrder = await parkingLotDbcontext.Orders.FirstOrDefaultAsync(_ => _.Id == id);
            if (parkingOrder != null)
            {
                return new ParkingOrderDto(parkingOrder);
            }
            else
            {
                return null;
            }
        }
    }
}
