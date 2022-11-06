using System;

namespace ParkingLotApi.Repository
{
    public class Order
    {

        public Order()
        {

        }

        public int Id { get; set; } 
        public string Name { get; set; }

        public string PalteNumber { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime CloseTime { get; set; }

        public bool OrderStatus { get; set; }
    }
}
