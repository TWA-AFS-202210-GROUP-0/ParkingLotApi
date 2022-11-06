using ParkingLotApi.Models;
using System.Collections.Generic;
using System.Linq;

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
        this.OrderDtos = parkingLotEntity.Orders != null 
            ? parkingLotEntity.Orders.Select(_ => new OrderDto(_)).ToList()
            : new List<OrderDto>();
    }

    public ParkingLotEntity ToEntity()
    {
        var parkingLotEntity = new ParkingLotEntity
        {
            Name = this.Name,
            Capacity = this.Capacity,
            Location = this.Location,
            Orders = this.OrderDtos?.Select(_ => _.ToEntity())
        };
        return parkingLotEntity;
    }

    public string Name { get; set; }
    public int Capacity { get; set; }
    public string Location { get; set; }
    public List<OrderDto>? OrderDtos { get; set; }
}