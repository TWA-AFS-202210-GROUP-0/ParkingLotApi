using ParkingLotApi.Models;
using System.Collections.Generic;
using System.Linq;

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
                if (parkingLotEntity.Availibility != null)
                {
                    this.Availibility = parkingLotEntity.Availibility;
                }

                this.Name = parkingLotEntity.Name;
                this.Location = parkingLotEntity.Location;
                this.ParkingOrderDto = parkingLotEntity.ParkingOrder?.Select(_ => new ParkingOrderDto(_)).ToList();
            }
        }

        public string Name { get; set; }

        public int Capacity { get; set; }

        public int? Availibility { get; set; }

        public string Location { get; set; }

        public List<ParkingOrderDto>? ParkingOrderDto { get; set; }

        public ParkingLotEntity ToEntity()
        {
            return new ParkingLotEntity
            {
                Capacity = this.Capacity,
                Name = this.Name,
                Location = this.Location,
                Availibility = this.Availibility != null ? this.Availibility : null,
                ParkingOrder = ParkingOrderDto?.Select(_ => _.ToEntity()).ToList(),
            };
        }
    }
}
