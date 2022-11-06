using System.Collections.Generic;
using System.Threading.Tasks;
using ParkingLotApi.Dto;
using ParkingLotApi.Repository;

namespace ParkingLotApi.Services
{
    public class ParkingLotService : IParkingLotService
    {
        private readonly ParkingDbContext _context;

        public ParkingLotService(ParkingDbContext parkingDbContext)
        {
            this._context = parkingDbContext;
        }

        public async Task<int> AddParkingLot(ParkingLotDto parkingLotDto)
        {
            return 1;
        }

        public async Task<ParkingLotDto> GetParkingLotById(int parkingLotId)
        {
            return new ParkingLotDto();
        }

        public async Task<ParkingLotDto> UpdateParkingLot(ParkingLotDto parkingLotDto)
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
