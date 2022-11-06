using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ParkingLotApi.Dtos;
using ParkingLotApi.Models;
using ParkingLotApi.Repository;

namespace ParkingLotApi.Services
{
    public class ParkingLotService : IParkingLotService
    {
        private readonly ParkingLotDbContext _parkingLotDbContext;
        public ParkingLotService(ParkingLotDbContext parkingLotDbContext)
        {
            _parkingLotDbContext = parkingLotDbContext;
        }

        public async Task<List<ParkingLotDto>> GetAll()
        {
            var parkingLots= _parkingLotDbContext.ParkingLots;
            return parkingLots.Select(parkingLotEntity => new ParkingLotDto(parkingLotEntity)).ToList();
        }

        public async Task<int> AddParkingLot(ParkingLotDto parkingLotDto)
        {
            var parkingLotEntity = parkingLotDto.ToEntity();
            var foundParkingLotEntity = _parkingLotDbContext.ParkingLots.FirstOrDefault(_ => _.Name == parkingLotEntity.Name);
            if (foundParkingLotEntity != null)
            {
                return -1;
            }
            _parkingLotDbContext.AddAsync(parkingLotEntity);
            _parkingLotDbContext.SaveChangesAsync();
            return parkingLotEntity.Id;
        }

        public async Task<ParkingLotDto> GetParkingLotById(int id)
        {
            var foundParkingLotEntity = _parkingLotDbContext.ParkingLots.FirstOrDefault(_ => _.Id == id);
            if (foundParkingLotEntity == null)
            {
                return null;
            }

            return new ParkingLotDto(foundParkingLotEntity);
        }

        public async Task<ParkingLotDto> DeleteParkingLotById(int id)
        {
            var foundParkingLotEntity = _parkingLotDbContext.ParkingLots.FirstOrDefault(_ => _.Id == id);
            if (foundParkingLotEntity == null)
            {
                return null;
            }

            _parkingLotDbContext.Remove(foundParkingLotEntity);
            _parkingLotDbContext.SaveChangesAsync();
            return new ParkingLotDto(foundParkingLotEntity);
        }

        public async Task<List<ParkingLotDto>> GetParkingLotByPage(int page)
        {
            var total = _parkingLotDbContext.ParkingLots.Count();
            var start = (page - 1) * 15 + 1;
            var end = page * 15;
            var numberOfParkingLot = Math.Min(end - start + 1, total - start + 1);
            if (total < start)
            {
                return null;
            }

            var parkingLotEntities = _parkingLotDbContext.ParkingLots.Skip(start - 1).Take(numberOfParkingLot).ToList();
            return parkingLotEntities.Select(_ => new ParkingLotDto(_)).ToList();
        }

        public async Task<ParkingLotDto> UpdateParkingLotCapacity(int id, int newCapacity)
        {
            var parkingLotEntityFound = _parkingLotDbContext.ParkingLots.FirstOrDefault(_ => _.Id == id);
            if (parkingLotEntityFound == null)
            {
                return null;
            }

            parkingLotEntityFound.Capacity = newCapacity;
            var parkingLotEntity = _parkingLotDbContext.ParkingLots.Update(parkingLotEntityFound);
            _parkingLotDbContext.SaveChangesAsync();
            return new ParkingLotDto(parkingLotEntityFound);
        }
    }
}
