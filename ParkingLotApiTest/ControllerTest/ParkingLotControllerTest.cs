using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ParkingLotApi;
using ParkingLotApi.Dto;
using Xunit;

namespace ParkingLotApiTest.ControllerTest
{
    using Microsoft.AspNetCore.Mvc.Testing;

    public class ParkingLotControllerTest
    {
        public ParkingLotControllerTest()
        {
        }

        public HttpClient GetClient()
        {
            var factory = new WebApplicationFactory<Program>();
            return factory.CreateClient();
        }

        public async Task Should_create_new_parking_lot()
        {
            //Given
            var client = GetClient();
            var newCompany = new ParkingLotDto()
            {
                Name = "SLB",
                Capacity = 10,
                Location = "TUSPark"
            };
            //When
            var response = await client.PostAsJsonAsync("/parkinglots", newCompany);
            //Then
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }
    }
}