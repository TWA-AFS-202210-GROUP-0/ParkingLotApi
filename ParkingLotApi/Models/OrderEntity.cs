using System;

namespace ParkingLotApi.Models
{
    public class OrderEntity
    {
        public OrderEntity()
        {
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string PlateNumber { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime? CloseTime { get; set; }
        public bool OrderStatus { get; set; }
    }
}
