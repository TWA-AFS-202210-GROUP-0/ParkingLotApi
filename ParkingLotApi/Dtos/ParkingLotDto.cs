using ParkingLotApi.Models;
using System;

namespace ParkingLotApi.Dtos
{
    public class ParkingLotDto
    {
        private ParkingLotEntity parkingLotEntity;

        public ParkingLotDto()
        {
        }
        public string Name { get; set; }
        public int Capacity { get; set; }
        public string Location { get; set; }

        public ParkingLotEntity ToEntity()
        {
            var parkingLotEntity = new ParkingLotEntity();
            parkingLotEntity.Name = Name;
            parkingLotEntity.Capacity = Capacity;
            parkingLotEntity.Location = Location;
            return parkingLotEntity;
        }
    }
}