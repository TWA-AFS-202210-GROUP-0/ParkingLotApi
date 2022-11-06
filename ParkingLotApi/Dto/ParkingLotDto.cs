using ParkingLotApi.Repository;
using System;

namespace ParkingLotApi.Dto
{
    public class ParkingLotDto
    {

        public ParkingLotDto()
        {

        }

        public ParkingLotDto(ParkingLot c)
        {
           this.Name = c.Name; 
           this.Capacity = c.Capacity;
           this.Location = c.Location;
        }

        public string Name { get; set; }

        public int Capacity { get; set; }
        public string Location { get; set; }

        public ParkingLot ToEntity()
        {
            return new ParkingLot()
            {
                Name = this.Name,
                Capacity = this.Capacity,
                Location = this.Location
            };
        }
    }
}
