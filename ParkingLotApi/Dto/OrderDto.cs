using ParkingLotApi.Repository;
using System;

namespace ParkingLotApi.Dto
{
    public class OrderDto
    {

        public OrderDto(Order order)
        {
            this.Name = order.Name;
            this.PalteNumber = order.PalteNumber;
            this.CreateTime = order.CreateTime;
            this.CloseTime = order.CloseTime;
            this.OrderStatus = order.OrderStatus;
        }

        public OrderDto()
        {
        }

        public string Name { get; set; }

        public string PalteNumber { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime CloseTime { get; set; }

        public bool OrderStatus { get; set; }

        public Order toEntity()
        {
            return new Order()
            {
                Name = this.Name,
                PalteNumber = this.PalteNumber,
                CreateTime = this.CreateTime,
                CloseTime = this.CloseTime,
                OrderStatus = this.OrderStatus,
            };
        }
    }
}