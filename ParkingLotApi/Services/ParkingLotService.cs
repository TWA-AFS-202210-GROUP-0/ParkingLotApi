using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ParkingLotApi.Dto;
using ParkingLotApi.Repository;

namespace ParkingLotApi.Services
{
    public class ParkingLotService : IParkingLotService
    {
        private readonly ParkingDbContext _parkingDbContext;

        public ParkingLotService(ParkingDbContext parkingDbParkingDbContext)
        {
            this._parkingDbContext = parkingDbParkingDbContext;
        }

        public async Task<int> AddParkingLot(ParkingLotDto parkingLotDto)
        {
            if(_parkingDbContext.ParkingLots.ToList().FirstOrDefault(e => e.Name == parkingLotDto.Name) != null)
            {
                throw new DuplicateNameException();
            }
            var parkingLotEntity = DtoConverter.ToEntity(parkingLotDto);
            await _parkingDbContext.ParkingLots.AddAsync(parkingLotEntity);
            await _parkingDbContext.SaveChangesAsync();
            return parkingLotEntity.Id;
        }

        public async Task<ParkingLotDto> GetParkingLotById(int parkingLotId)
        {
            var parkingLotEntity = await _parkingDbContext.ParkingLots.SingleOrDefaultAsync(e => e.Id == parkingLotId);
            if (parkingLotEntity !=null)
            {
                return DtoConverter.ToDto(parkingLotEntity);
            }

            throw new NullReferenceException();
        }

        public async Task<ParkingLotDto> UpdateParkingLot(int id, ParkingLotDto parkingLotDto)
        {
            {
                var entityById = await _parkingDbContext.ParkingLots.FirstOrDefaultAsync(e => e.Id == id);
                if (entityById == null)
                {
                    throw new NullReferenceException();
                }
                entityById.Capacity = parkingLotDto.Capacity;
                await _parkingDbContext.SaveChangesAsync();

                return DtoConverter.ToDto(entityById);
            }
        }

        public async Task<List<ParkingLotDto>> GetMultiParkingLots(int skip, int take)
        {
            var parkingLotEntitiess = _parkingDbContext.ParkingLots.OrderBy(e => e.Id).Skip(skip).Take(take);
                var parkingLotDtos = parkingLotEntitiess.Select(e => DtoConverter.ToDto(e)).ToList();
            return parkingLotDtos;
        }

        public async Task DeleteParkingLot(int id, string name)
        {
            var entityById = await _parkingDbContext.ParkingLots.FirstOrDefaultAsync(e => e.Id == id);
            if (entityById == null || entityById.Name != name)
            {
                throw new NullReferenceException();
            }

            _parkingDbContext.Remove(entityById);
            await _parkingDbContext.SaveChangesAsync();
            return;
        }
    }
}
