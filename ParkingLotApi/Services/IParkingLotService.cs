using System.Collections.Generic;
using ParkingLotApi.Dtos;

namespace ParkingLotApi.Services;

public interface IParkingLotService
{
    List<ParkingLotDto> GetAll();
}