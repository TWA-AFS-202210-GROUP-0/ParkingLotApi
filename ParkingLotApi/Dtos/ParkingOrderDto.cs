using ParkingLotApi.Models;
using System;

namespace ParkingLotApi.Dtos
{
    public class ParkingOrderDto
    {
        private ParkingOrderEntity foundParkingOrder;

        public ParkingOrderDto()
        {
        }

        public ParkingOrderDto(ParkingOrderEntity foundParkingOrder)
        {
            PlateNumber = foundParkingOrder.PlateNumber;
            ParkingLotName = foundParkingOrder.ParkingLotName;
            CreateTime = foundParkingOrder.CreateTime;
            CloseTime = foundParkingOrder.CloseTime;
            OrderStatus = foundParkingOrder.OrderStatus;
        }

        public string ParkingLotName { get; set; }
        public string PlateNumber { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? CloseTime { get; set; }
        public bool OrderStatus { get; set; }

        public ParkingOrderEntity ToEntity()
        {
            var parkingOrderEntity = new ParkingOrderEntity();
            parkingOrderEntity.ParkingLotName = ParkingLotName;
            parkingOrderEntity.PlateNumber = PlateNumber;
            parkingOrderEntity.CreateTime = CreateTime;
            parkingOrderEntity.CloseTime = CloseTime;
            parkingOrderEntity.OrderStatus = OrderStatus;
            
            return parkingOrderEntity;
        }
    }

}
