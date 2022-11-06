using ParkingLotApi.Repository;
using System;

namespace ParkingLotApi
{
    public class ParkingLotDTO
    {
        private ParkingLotEntity p;

        public ParkingLotDTO()
        {
        }

        public ParkingLotDTO(ParkingLotEntity parkingLotEntity)
        {
            Name = parkingLotEntity.Name;
            Capacity = parkingLotEntity.Capacity;
            Location = parkingLotEntity.Location;
        }

        public string Name { get; set; }

        public int Capacity { get; set; }

        public string Location { get; set; }

        internal ParkingLotEntity ToEntity()
        {
            return new ParkingLotEntity() {
                Name = this.Name,
                Capacity = this.Capacity,
                Location = this.Location,
            };
        }
    }
}