using ParkingLotApi.Models;

namespace ParkingLotApi.Dtos;

public class ParkingLotDto
{
    public ParkingLotDto()
    {
    }

    public ParkingLotDto(ParkingLotEntity parkingLotEntity)
    {
        this.Name = parkingLotEntity.Name;
        this.Capacity = parkingLotEntity.Capacity;
        this.Location = parkingLotEntity.Location;
    }

    public string Name { get; set; }
    public int Capacity { get; set; }
    public string Location { get; set; }

    public ParkingLotEntity ToEntity()
    {
        var parkingLotEntity = new ParkingLotEntity
        {
            Name = this.Name,
            Capacity = this.Capacity,
            Location = this.Location
        };
        return parkingLotEntity;
    }
}