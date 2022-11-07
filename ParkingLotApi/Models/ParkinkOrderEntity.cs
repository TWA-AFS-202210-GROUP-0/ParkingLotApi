using System;

namespace ParkingLotApi.Models
{
    public class ParkingOrderEntity
    {
        public ParkingOrderEntity()
        {
        }
        public int Id { get; set; }
        public string ParkingLotName { get; set; }
        public string PlateNumber { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? CloseTime { get; set; }
        public bool OrderStatus { get; set; }
    }
}
