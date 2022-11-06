using System.Threading.Tasks;
using ParkingLotApi.Dto;

namespace ParkingLotApi.Services;

public interface IParkingOrderService
{
    Task<ParkingOrderDto> CreateOrder(int parkingLotId, ParkingOrderDto parkingOrderDto);
    Task<ParkingOrderDto> CloseOrder(int parkingLotId, ParkingOrderDto parkingOrderDto);
}