using System;

namespace ParkingLotApi
{
    public class OrderDTO
    {
        private OrderEntity orderEntity;

        public OrderDTO()
        {

        }

        public OrderDTO(OrderEntity orderEntity)
        {
            OrderNumber = orderEntity.OrderNumber;
            ParkingLotName = orderEntity.ParkingLotName;
            PlateNumber = orderEntity.PlateNumber;
            CreationTime = orderEntity.CreationTime;
            ClosedTime = orderEntity.ClosedTime;
            Status = orderEntity.Status ? "open" : "closed";
        }

        public string OrderNumber { get; set; }

        public string ParkingLotName { get; set; }

        public string PlateNumber { get; set; }

        public DateTime CreationTime { get; set; }

        public DateTime? ClosedTime { get; set; }

        public string Status { get; set; }

        public OrderEntity ToEntity()
        {
            return new OrderEntity
            {
                OrderNumber = OrderNumber,
                ParkingLotName = ParkingLotName,
                PlateNumber = PlateNumber,
                CreationTime = CreationTime,
                ClosedTime = ClosedTime,
                Status = Status == "open" ? true : false,
            };
        }

    }
}