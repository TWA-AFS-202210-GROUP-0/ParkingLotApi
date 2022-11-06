using System;
using System.Linq;
using ParkingLotApi.Repository;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ParkingLotApi.Dtos;
using ParkingLotApi.Models;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Collections.Generic;

namespace ParkingLotApi.Services
{
    public class ParkingLotService
    {
        private readonly ParkingLotContext parkingLotDbcontext;

        public ParkingLotService(ParkingLotContext parkingLotDbcontext)
        {
            this.parkingLotDbcontext = parkingLotDbcontext;
        }

        public async Task<ParkingLotEntity> AddNewParkingLot(ParkingLotDto parkingLotDto)
        {
            if ((parkingLotDbcontext.ParkingLots.Where(parkinglot => parkinglot.Name == parkingLotDto.Name).ToList()
                    .Count == 0) && (parkingLotDto.Capacity > 0))
            {
                //1. convert dto to entity
                ParkingLotEntity entity = parkingLotDto.ToEntity();
                //2. save entity to db
                await parkingLotDbcontext.ParkingLots.AddAsync(entity);
                await parkingLotDbcontext.SaveChangesAsync();
                //3. return company id
                return entity;
            }
            else
            {
                return null;
            }
        }

        public async Task<ParkingLotDto> GetById(int id)
        {
            var parkingLot = await parkingLotDbcontext.ParkingLots.FirstOrDefaultAsync(
                parkinglot => parkinglot.Id == id);
            return new ParkingLotDto(parkingLot);
        }

        public async Task DeleteParkingLot(int id)
        {
            var parkinglot = await parkingLotDbcontext.ParkingLots
                .FirstOrDefaultAsync(parkinglot => parkinglot.Id == id);
            parkingLotDbcontext.ParkingLots.Remove(parkinglot);
            await parkingLotDbcontext.SaveChangesAsync();
        }

        public List<ParkingLotDto> GetParkingLots(int? pageIndex)
        {
            var parkingLotEntities = parkingLotDbcontext.ParkingLots.ToList();
            var parkingLotDtos = parkingLotEntities.Select(parkinglotEntity => new ParkingLotDto(parkinglotEntity)).ToList();
            if (pageIndex != null)
            {
                int lower = (pageIndex.Value - 1) * 15;
                int upper = lower + 15;
                if (upper <= parkingLotDbcontext.ParkingLots.Count())
                {
                    return parkingLotDtos.Skip(lower).Take(15).ToList();
                }
                else
                {
                    return parkingLotDtos.Skip(lower).Take(15 - upper + parkingLotDtos.Count).ToList();
                }
            }
            else
            {
                return parkingLotDtos;
            }
        }
    }
}
