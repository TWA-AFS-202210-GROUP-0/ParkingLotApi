using System.ComponentModel.DataAnnotations;

namespace ParkingLotApi.Dto
{
    public class ParkingLotDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public int Capacity { get; set; }
        [Required]
        public string Location { get; set; }
    }
}
