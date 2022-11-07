using ParkingLotApi.Models;
using System;

namespace ParkingLotApi.Dtos
{
    public class OrderDto
    {
        public OrderDto()
        {
        }

        public OrderDto(OrderEntity orderEntity)
        {
            this.Name = orderEntity.Name;
            this.PlateNumber = orderEntity.PlateNumber;
            this.CreationTime = orderEntity.CreationTime;
            this.CloseTime = orderEntity.CloseTime;
            this.OrderStatus = orderEntity.OrderStatus;
        }

        public OrderEntity ToEntity()
        {
            var orderEntity = new OrderEntity()
            {
                Name = this.Name,
                PlateNumber = this.PlateNumber,
                CreationTime = this.CreationTime,
                CloseTime = this.CloseTime,
                OrderStatus = this.OrderStatus
            };
            return orderEntity;
        }

        public string Name { get; set; }
        public string PlateNumber { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime? CloseTime { get; set; }
        public bool OrderStatus { get; set; }
    }
}
