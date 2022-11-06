using System.Linq;
using ParkingLotApi.Repository;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ParkingLotApi.Dtos;
using ParkingLotApi.Models;

namespace ParkingLotApi.Services
{
    public class ParkingLotService
    {
        private readonly ParkingLotContext parkingLotDbcontext;

        public ParkingLotService(ParkingLotContext parkingLotDbcontext)
        {
            this.parkingLotDbcontext = parkingLotDbcontext;
        }

        public async Task<int> AddNewParkingLot(ParkingLotDto parkingLotDto)
        {
            //1. convert dto to entity
            ParkingLotEntity entity = parkingLotDto.ToEntity();
            //2. save entity to db
            await parkingLotDbcontext.ParkingLots.AddAsync(entity);
            await parkingLotDbcontext.SaveChangesAsync();
            //3. return company id
            return entity.Id;
        }

        public async Task<ParkingLotDto> GetById(int id)
        {
            var parkingLot = await parkingLotDbcontext.ParkingLots.FirstOrDefaultAsync(
                parkinglot => parkinglot.Id == id);
            return new ParkingLotDto(parkingLot);
        }
    }
}
