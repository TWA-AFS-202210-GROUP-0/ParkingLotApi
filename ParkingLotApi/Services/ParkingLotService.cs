using System.Collections.Generic;
using System.Linq;
using ParkingLotApi.Dtos;
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

        public List<ParkingLotDto> GetAll()
        {
            var parkingLots= _parkingLotDbContext.ParkingLots;
            return parkingLots.Select(parkingLotEntity => new ParkingLotDto(parkingLotEntity)).ToList();
        }

        public ParkingLotDto AddParkingLot(ParkingLotDto parkingLotDto)
        {
            var parkingLotEntity = parkingLotDto.ToEntity();
            _parkingLotDbContext.AddAsync(parkingLotEntity);
            _parkingLotDbContext.SaveChangesAsync();
            return parkingLotDto;
        }
    }
}
