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

    }
}