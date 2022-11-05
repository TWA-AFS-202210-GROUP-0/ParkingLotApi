namespace ParkingLotApiTest.ControllerTest
{
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Newtonsoft.Json;
    using ParkingLotApi;
    using ParkingLotApi.Dtos;
    using Xunit;

    [Collection("Sequential")]

    public class ParkingLotControllerTest:TestBase
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
            var parkingLot = new ParkingLotDto
            {
                Name = "ParkingLotA",
                Capacity = 50,
                Location = "Street A",
            };
            var parkingLotContent = BuildRequestBody(parkingLot);

            //when
            var response = await client.PostAsync("/parkingLots", parkingLotContent);

            //then
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var createdParkingLot = await DeserializeResponse<ParkingLotDto>(response);
            Assert.Equal("ParkingLotA", createdParkingLot.Name);
        }


        [Fact]
        public async Task Should_delete_parking_lot_success()
        {
            var client = GetClient();
            var parkingLot = new ParkingLotDto
            {
                Name = "ParkingLotA",
                Capacity = 50,
                Location = "Street A",
            };
            var parkingLotContent = BuildRequestBody(parkingLot);
            var response = await client.PostAsync("/parkingLots", parkingLotContent);

            //when
            await client.DeleteAsync(response.Headers.Location);

            //then
            var response2 = await client.PostAsync("/parkingLots", parkingLotContent);
            Assert.Equal(HttpStatusCode.Created, response2.StatusCode);

        }
        private static StringContent BuildRequestBody<T>(T requestObject)
        {
            var requestJson = JsonConvert.SerializeObject(requestObject);
            var requestBody = new StringContent(requestJson, Encoding.UTF8, "application/json");
            return requestBody;
        }

        private static async Task<T?> DeserializeResponse<T>(HttpResponseMessage response)
        {
            if (response.Content != null)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                var deserializeObject = JsonConvert.DeserializeObject<T>(responseBody);

                return deserializeObject;
            }

            else
            {
                return default;
            }
        }
    }
}