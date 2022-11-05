using ParkingLotApi.Models;
using System;

namespace ParkingLotApi.Dtos
{
    public class ParkingLotNoLocationDto
    {
        private ParkingLotEntity parkingLotEntity;

        public ParkingLotNoLocationDto()
        {
        }

        public ParkingLotNoLocationDto(ParkingLotEntity parkingLotEntity)
        {
            this.Name = parkingLotEntity.Name;
            this.Capacity = parkingLotEntity.Capacity;
        }
        
        public string Name { get; set; }
        public int Capacity { get; set; }

        public ParkingLotEntity ToEntity()
        {
            var parkingLotEntity = new ParkingLotEntity();
            parkingLotEntity.Name = Name;
            parkingLotEntity.Capacity = Capacity;
            return parkingLotEntity;
        }
    }
}