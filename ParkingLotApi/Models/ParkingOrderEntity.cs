using System;
using System.ComponentModel.DataAnnotations;

namespace ParkingLotApi.Models
{
    public class ParkingOrderEntity
    {
        public int Id { get; set; }
        public string NameOfParkingLot { get; set; }
        [StringLength(5)]
        public string PlateNumber { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime CloseTime { get; set; }
        public bool IsOpen { get; set; } = true;
    }
}
