using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParkingLotApi.Dto;
using ParkingLotApi.Services;

namespace ParkingLotApiTest.ServiceTest
{
    [Collection("sequence1")]
    public class ParkingOrderServiceTest : ServiceTestBase
    {
        [Fact]
        public async Task Should_create_order()
        {
            // given
            var parkingLotDto = new ParkingLotDto()
            {
                Name = "SLB",
                Capacity = 1,
                Location = "TUSPark",
            };
            var newParkingLotId = await parkingLotService.AddParkingLot(parkingLotDto);

            //When
            var parkingOrderDto = await parkingOrderService.CreateOrder(1, new ParkingOrderDto()
            {
                PlateNumber = "12345",
            });
            //Then
            Assert.Equal("SLB",parkingOrderDto.NameOfParkingLot);
        }

        [Fact]
        public async Task Should_throw_full()
        {
            // given
            var parkingLotDto = new ParkingLotDto()
            {
                Name = "SLB",
                Capacity = 1,
                Location = "TUSPark",
            };
            var newParkingLotId = await parkingLotService.AddParkingLot(parkingLotDto);

            //When
            var parkingOrderDto = await parkingOrderService.CreateOrder(1, new ParkingOrderDto()
            {
                PlateNumber = "12345",
            });
            //Then
            Assert.ThrowsAsync<NullReferenceException>(async () => await parkingLotService.AddParkingLot(parkingLotDto));
        }

        [Fact]
        public async Task Should_close_order()
        {
            // given
            var parkingLotDto = new ParkingLotDto()
            {
                Name = "SLB",
                Capacity = 1,
                Location = "TUSPark",
            };
            var newParkingLotId = await parkingLotService.AddParkingLot(parkingLotDto);

            //When
            var parkingOrderDto = await parkingOrderService.CreateOrder(1, new ParkingOrderDto()
            {
                PlateNumber = "12345",
            });

            var closeResult = await parkingOrderService.CloseOrder(newParkingLotId, parkingOrderDto);
            Assert.False(closeResult.IsOpen);
        }
    }
}
