using ParkingLotApi.Models;
using System;

namespace ParkingLotApi.Dtos
{
    public class ParkingOrderDto
    {
        public ParkingOrderDto()
        {
        }

        public ParkingOrderDto(ParkingOrderEntity parkingOrderEntity)
        {
            this.Number = parkingOrderEntity.Number;
            this.CarPlateNumber = parkingOrderEntity.CarPlateNumber;
            this.CreateTime = parkingOrderEntity.CreateTime;
            this.CloseTime = parkingOrderEntity.CloseTime;
            this.OrderStatus = parkingOrderEntity.OrderStatus;
            this.NameOfParkingLot = parkingOrderEntity.NameOfParkingLot;
        }

        public int Number { get; set; }
        public string? NameOfParkingLot { get; set; }
        public string CarPlateNumber { get; set; }
        public DateTime? CreateTime { get; set; }
        public DateTime? CloseTime { get; set; }
        public OrderStatus OrderStatus { get; set; } = OrderStatus.OPEN;

        public ParkingOrderEntity ToEntity()
        {
            return new ParkingOrderEntity
            {
                Number = this.Number,
                CarPlateNumber = this.CarPlateNumber,
                CreateTime = this.CreateTime,
                CloseTime = this.CloseTime,
                OrderStatus = this.OrderStatus,
                NameOfParkingLot = this.NameOfParkingLot,
            };
        }
    }
}
