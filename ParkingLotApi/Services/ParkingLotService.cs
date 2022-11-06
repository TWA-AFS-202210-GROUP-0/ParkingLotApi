using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
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
            return new ParkingLotDto();
        }

        public async Task<ParkingLotDto> UpdateParkingLot(int id, ParkingLotDto parkingLotDto)
        {
            return new ParkingLotDto();
        }

        public async Task<List<ParkingLotDto>> GetMultiParkingLots(int skip, int take)
        {
            return new List<ParkingLotDto>();
        }

        public async Task<string> DeleteParkingLot(int id, string name)
        {
            return name;
        }


    }
}
