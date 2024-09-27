using System;

namespace ParkingLotApi.Models
{
    public class ParkingOrderEntity
    {
        public ParkingOrderEntity()
        {
        }

        public int Id { get; set; }
        public int Number { get; set; }
        public string? NameOfParkingLot { get; set; }
        public string CarPlateNumber { get; set; }
        public DateTime? CreateTime { get; set; }
        public DateTime? CloseTime { get; set; }
        public OrderStatus OrderStatus { get; set; } = OrderStatus.OPEN;
    }
}
