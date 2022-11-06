using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ParkingLotApi.Models;

namespace ParkingLotApi.Dto
{
    public class DtoConverter
    {
        public static ParkingLotEntity ToEntity(ParkingLotDto parkingLotDto)
        {
            return new ParkingLotEntity()
            {
                Name = parkingLotDto.Name,
                Capacity = parkingLotDto.Capacity,
                Location = parkingLotDto.Location,
            };
        }

        public static ParkingLotDto ToDto(ParkingLotEntity parkingLotEntity)
        {
            return new ParkingLotDto()
            {
                Name = parkingLotEntity.Name,
                Capacity = parkingLotEntity.Capacity,
                Location = parkingLotEntity.Location,
            };
        }

        public static ParkingOrderEntity ToEntity(ParkingOrderDto parkingOrderDto)
        {
            return new ParkingOrderEntity()
            {
                PlateNumber = parkingOrderDto.PlateNumber,
            };
        }

        public static ParkingOrderDto ToDto(ParkingOrderEntity parkingOrderEntity)
        {
            return new ParkingOrderDto()
            {
                Id = parkingOrderEntity.Id,
                PlateNumber = parkingOrderEntity.PlateNumber,
                NameOfParkingLot = parkingOrderEntity.NameOfParkingLot,
                CreateTime = parkingOrderEntity.CreateTime,
                CloseTime = parkingOrderEntity.CloseTime,
                IsOpen = parkingOrderEntity.IsOpen,
            };
        }
    }
}
