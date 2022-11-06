using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParkingLotApi.Dto;

namespace ParkingLotApiTest.ServiceTest
{
    [Collection("sequence1")]
    public class ParkingLotServiceTest : ServiceTestBase
    {
        [Fact]
        public async void Should_create_parking_lot_()
        {
            // given
            var parkingLotDto = new ParkingLotDto()
            {
                Name = "SLB",
                Capacity = 10,
                Location = "TUSPark",
            };

            // when
            var newParkingLotDto = await ParkingLotService.AddParkingLot(parkingLotDto);

            // then
            Assert.Equal(1, ParkingDbContext.ParkingLots.ToList().Count);
            Assert.Equal(parkingLotDto.Name, ParkingDbContext.ParkingLots.ToList()[0].Name);
            Assert.Equal(newParkingLotDto, ParkingDbContext.ParkingLots.ToList()[0].Id);
        }

        [Fact]
        public async void Should_not_create_parking_lot_when_name_conflicts()
        {
            // given
            var parkingLotDto = new ParkingLotDto()
            {
                Name = "SLB",
                Capacity = 10,
                Location = "TUSPark",
            };

            // when
            var newParkingLotDto = await ParkingLotService.AddParkingLot(parkingLotDto);
            // Then
            Assert.ThrowsAsync<DuplicateNameException>(async () => await ParkingLotService.AddParkingLot(parkingLotDto));
            Assert.Equal(1, ParkingDbContext.ParkingLots.Count());
        }

        [Fact]
        public async void Should_Get_parkingLot_by_id()
        {
            // given
            var parkingLotDto = new ParkingLotDto()
            {
                Name = "SLB",
                Capacity = 10,
                Location = "TUSPark",
            };
            var newParkingLotDto = await ParkingLotService.AddParkingLot(parkingLotDto);
            // when
            var getParkingLotDto = await ParkingLotService.GetParkingLotById(newParkingLotDto);
            // Then
            Assert.Equal(parkingLotDto.Name, getParkingLotDto.Name);
        }


        [Fact]
        public async void Should_throw_null_when_get_parkingLot_by_wrong_id()
        {
            // given
            var parkingLotDto = new ParkingLotDto()
            {
                Name = "SLB",
                Capacity = 10,
                Location = "TUSPark",
            };
            var newParkingLotDto = await ParkingLotService.AddParkingLot(parkingLotDto);
            // Then
            Assert.ThrowsAsync<NullReferenceException>(async () =>
                await ParkingLotService.GetParkingLotById(newParkingLotDto - 1));
        }
    }
}
