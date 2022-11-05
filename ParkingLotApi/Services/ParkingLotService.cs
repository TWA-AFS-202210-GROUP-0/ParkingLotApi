using ParkingLotApi.Dtos;
using ParkingLotApi.Exceptions;
using ParkingLotApi.Models;
using ParkingLotApi.Repository;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ParkingLotApi.Services
{
    public class ParkingLotService
    {
        private readonly ParkingLotContext parkingLotContext;

        public ParkingLotService(ParkingLotContext parkingLotContext)
        {
            this.parkingLotContext = parkingLotContext;
        }

        public async Task<int> AddParkingLot(ParkingLotDto parkingLotDto)
        {
            ParkingLotEntity parkingLotEntity = parkingLotDto.ToEntity();
            if (parkingLotContext.ParkingLots.ToList().Exists(_ => _.Name.Equals(parkingLotDto.Name)))
            {
                throw new WrongNameException("This parking lot has been registered");
            }

            if (parkingLotDto.Capacity < 0)
            {
                throw new WrongCapacityException("Capacity cannot be minus");
            }

            await parkingLotContext.ParkingLots.AddAsync(parkingLotEntity);
            await parkingLotContext.SaveChangesAsync();
            return parkingLotEntity.Id;
        }

        public async Task DeleteParkingLot(int id)
        {
            var foundParkingLot = parkingLotContext.ParkingLots
                .FirstOrDefault(_ => _.Id == id);
            parkingLotContext.ParkingLots.Remove(foundParkingLot);
            await parkingLotContext.SaveChangesAsync();
        }
    }
}
