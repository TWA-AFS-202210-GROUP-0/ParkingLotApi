using System;
using System.ComponentModel.DataAnnotations;

namespace ParkingLotApi.Dto
{
    public class ParkingOrderDto
    {
        public int? Id { get; set; }
        public string? NameOfParkingLot { get; set; }

        [StringLength(5)]
        [Required]
        public string PlateNumber { get; set; }

        public DateTime? CreateTime { get; set; }

        public DateTime? CloseTime { get; set; }

        public bool IsOpen { get; set; } = true;
    }
}
