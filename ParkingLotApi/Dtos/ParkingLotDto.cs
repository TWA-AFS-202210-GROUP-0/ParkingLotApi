using ParkingLotApi.Models;

namespace ParkingLotApi.Dtos
{
    public class ParkingLotDto
    {
        public ParkingLotDto()
        {
        }

        public ParkingLotDto(ParkingLotEntity parkingLotEntity)
        {
            if (parkingLotEntity.Capacity != null)
            {
                this.Capacity = parkingLotEntity.Capacity;
                this.Name = parkingLotEntity.Name;
                this.Location = parkingLotEntity.Location;
            }
        }

        public string Name { get; set; }

        public int? Capacity { get; set; }

        public string Location { get; set; }
    }
}
