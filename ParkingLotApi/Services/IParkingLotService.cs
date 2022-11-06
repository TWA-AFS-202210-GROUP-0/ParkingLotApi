using System.Collections.Generic;
using System.Threading.Tasks;
using ParkingLotApi.Dto;

namespace ParkingLotApi.Services;

public interface IParkingLotService
{
    Task<int> AddParkingLot(ParkingLotDto parkingLotDto);
    Task<ParkingLotDto> GetParkingLotById(int parkingLotId);
    Task<ParkingLotDto> UpdateParkingLot(int id, ParkingLotDto parkingLotDto);
    Task<List<ParkingLotDto>> GetMultiParkingLots(int skip, int take);
    Task<string> DeleteParkingLot(int id, string name);
}