using System;

namespace ParkingLotApi
{
    public class OrderEntity
    {
        public OrderEntity()
        {

        }

        public int Id { get; set; }

        public string OrderNumber { get; set; }

        public string ParkingLotName { get; set; }

        public string PlateNumber { get; set; }

        public DateTime CreationTime { get; set; }

        public DateTime? ClosedTime { get; set; }

        public bool Status { get; set; }

    }
}