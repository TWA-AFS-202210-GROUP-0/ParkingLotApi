using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ParkingLotApi.Dto
{
    public class ParkingLotDto
    {
        public int? Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int Capacity { get; set; }
        [Required]
        public string Location { get; set; }

        public List<ParkingOrderDto>? ParkingOrderDtos { get; set; }
    }
}
