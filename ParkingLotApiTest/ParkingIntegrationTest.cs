using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ParkingLotApi;
using ParkingLotApi.Dto;
using Xunit;

namespace ParkingLotApiTest.ControllerTest
{
    using Microsoft.AspNetCore.Mvc.Testing;

    public class ParkingIntegrationTest
    {
        public ParkingIntegrationTest()
        {
        }

        public HttpClient GetClient()
        {
            var factory = new WebApplicationFactory<Program>();
            return factory.CreateClient();
        }

        [Fact]
        public async Task Should_create_new_parking_lot()
        {
            //Given
            var client = GetClient();
            var newParkingLot = new ParkingLotDto()
            {
                Name = "SLB",
                Capacity = 10,
                Location = "TUSPark"
            };
            //When
            var response = await client.PostAsJsonAsync("/parkinglots", newParkingLot);
            var body = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ParkingLotDto>(body);
            //Then
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.Equal("SLB", result.Name);
        }

        [Fact]
        public async Task Should_get_by_page()
        {
            //Given
            var client = GetClient();
            var newParkingLot = new ParkingLotDto()
            {
                Name = "SLB",
                Capacity = 10,
                Location = "TUSPark"
            };
            //When
            var response = await client.PostAsJsonAsync("/parkinglots", newParkingLot);
            //Then
            var getResponse = await client.GetAsync($"/parkinglots?pageIndex=1");
            Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);
            var body = await getResponse.Content.ReadAsStringAsync();
            var result = await JsonConvert.DeserializeObjectAsync<List<ParkingLotDto>>(body);
            Assert.Equal(1, result.Count);
        }

        [Fact]
        public async Task Should_create_parking_order()
        {
            //Given
            var client = GetClient();
            var newParkingLot = new ParkingLotDto()
            {
                Name = "SLB",
                Capacity = 10,
                Location = "TUSPark"
            };
            var response = await client.PostAsJsonAsync("/parkinglots", newParkingLot);
            //Given 2
            var createResponse = await client.PostAsJsonAsync("parkinglots/1/parkingorders/",
                new ParkingOrderDto() { PlateNumber = "12345" });
            var body = await createResponse.Content.ReadAsStringAsync();
            var result = await JsonConvert.DeserializeObjectAsync<ParkingOrderDto>(body);
            Assert.True(result.IsOpen);
        }
    }
}