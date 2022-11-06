using ParkingLotApi.Dtos;
using ParkingLotApi.Exceptions;
using ParkingLotApi.Models;
using ParkingLotApi.Repository;
using System;
using System.Collections.Generic;
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
                .FirstOrDefault(_ => _.Id.Equals(id));
            parkingLotContext.ParkingLots.Remove(foundParkingLot);
            await parkingLotContext.SaveChangesAsync();
        }

        public async Task<List<ParkingLotNoLocationDto>> GetParkingLotByPage(int pageIndex)
        {
            int pageSize = 15;
            int parkingLotCount = parkingLotContext.ParkingLots.Count();
            int beginIndex = (pageIndex - 1) * pageSize;
            int pageIndexCount = Math.Min(pageSize, parkingLotCount - (pageIndex - 1) * pageSize);
            var parkingLots = parkingLotContext.ParkingLots.OrderBy(_ => _.Id).ToList().GetRange(beginIndex, pageIndexCount);
            return parkingLots.Select(parkingLot => new ParkingLotNoLocationDto(parkingLot)).ToList();
        }

        public async Task<ParkingLotDto> ModifyParkingLotCapacity(ParkingLotDto parkingLotDto)
        {
            var foundParkingLot = parkingLotContext.ParkingLots
                .FirstOrDefault(_ => _.Name.Equals(parkingLotDto.Name));
            foundParkingLot.Capacity=parkingLotDto.Capacity;
            return new ParkingLotDto(foundParkingLot);
        }

        public async Task<ParkingLotDto> GetParkingLotById(int id)
        {
            var foundParkingLot = parkingLotContext.ParkingLots
                .FirstOrDefault(_ => _.Id.Equals(id));
            return new ParkingLotDto(foundParkingLot);
        }
    }
}
