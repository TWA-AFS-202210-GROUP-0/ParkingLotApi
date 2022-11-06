using System.Threading.Tasks;
using ParkingLotApi;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Collections.Generic;
using System.Linq;

namespace ParkingLotApiTest.ControllerTest
{
    [Collection("IntegrationTest")]
    public class ParkingLotControllerTest: TestBase
    {
        public ParkingLotControllerTest(CustomWebApplicationFactory<Program> factory)
            : base(factory)
        {
        }

        [Fact]
        public async Task Should_add_parking_lot_successfully()
        {
            // given
            var client = GetClient();
            ParkingLotDTO parkingLotDTO = new ParkingLotDTO
            {
                Name = "hi",
                Capacity = 1,
                Location = "hihi",
            };
            var requestBody = CreateRequestBody(parkingLotDTO);

            // when
            await client.PostAsync("/parkinglots", requestBody);

            // then
            var allParkingLotsResponse = await client.GetAsync("/parkinglots");
            var parkingLots = await DeserializeResponse<List<ParkingLotDTO>>(allParkingLotsResponse);
            Assert.Single(parkingLots);
        }

        [Fact]
        public async Task Should_sold_parking_lot_successfully()
        {
            // given
            var client = GetClient();
            ParkingLotDTO parkingLotDTO = new ParkingLotDTO
            {
                Name = "hi",
                Capacity = 1,
                Location = "hihi",
            };
            var requestBody = CreateRequestBody(parkingLotDTO);
            var response = await client.PostAsync("/parkinglots", requestBody);
            var id = await DeserializeResponse<int>(response);

            //when
            await client.DeleteAsync($"/parkinglots/{id}");

            // then
            var allParkingLotsResponse = await client.GetAsync("/parkinglots");
            var parkingLots = await DeserializeResponse<List<ParkingLotDTO>>(allParkingLotsResponse);
            Assert.Equal(0, parkingLots.Count());
        }

        [Fact]
        public async Task Should_get_parking_lots_by_page()
        {
            // given
            var client = GetClient();
            for (var i = 0; i < 18; i++)
            {
                ParkingLotDTO parkingLotDTO = new ParkingLotDTO()
                {
                    Name = "hi" + i,
                    Capacity = 1,
                    Location = "hihi",
                };
                var requestBody = CreateRequestBody(parkingLotDTO);
                await client.PostAsync("/parkinglots", requestBody);
            }

            // when
            var page2ParkingLotsResponse = await client.GetAsync("/parkinglots?pageNumber=2");

            // then
            var parkingLots = await DeserializeResponse<List<ParkingLotDTO>>(page2ParkingLotsResponse);
            Assert.Equal(3, parkingLots.Count());
        }

        [Fact]
        public async Task Should_get_one_parking_lot_successfully()
        {
            // given
            var client = GetClient();
            ParkingLotDTO parkingLotDTO = new ParkingLotDTO
            {
                Name = "hi",
                Capacity = 1,
                Location = "hihi",
            };
            var requestBody = CreateRequestBody(parkingLotDTO);
            var response = await client.PostAsync("/parkinglots", requestBody);
            var id = await DeserializeResponse<int>(response);

            //when
            var getResponse = await client.GetAsync($"/parkinglots/{id}");

            // then
            var parkingLot = await DeserializeResponse<ParkingLotDTO>(getResponse);
            Assert.Equal(parkingLotDTO.Name, parkingLot.Name);
            Assert.Equal(parkingLotDTO.Capacity, parkingLot.Capacity);
            Assert.Equal(parkingLotDTO.Location, parkingLot.Location);
        }

    }
}