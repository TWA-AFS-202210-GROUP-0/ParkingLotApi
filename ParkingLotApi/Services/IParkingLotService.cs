using System.Collections.Generic;
using System.Threading.Tasks;
using ParkingLotApi.Dtos;

namespace ParkingLotApi.Services;

public interface IParkingLotService
{
    Task<List<ParkingLotDto>> GetAll();
    Task<int> AddParkingLot(ParkingLotDto parkingLotDto);
    Task<ParkingLotDto> DeleteParkingLotById(int id);
    Task<ParkingLotDto> GetParkingLotById(int id);
    Task<List<ParkingLotDto>> GetParkingLotByPage(int page);
    Task<ParkingLotDto> UpdateParkingLotCapacity(int id, int newCapacity);
}