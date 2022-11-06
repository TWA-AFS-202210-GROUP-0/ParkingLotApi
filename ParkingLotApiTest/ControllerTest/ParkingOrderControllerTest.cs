namespace ParkingLotApiTest.ControllerTest
{
    using System.Collections.Generic;
    using System.Linq;
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

    public class ParkingOrderControllerTest:TestBase
    {
        
        public ParkingOrderControllerTest(CustomWebApplicationFactory<Program> factory)
            : base(factory)
        {
        }

        [Fact]
        public async Task Should_add_parking_order_successfully()
        {
            // given
            var client = GetClient();
            var parkingOrder = new ParkingOrderDto
            {
                ParkingLotName = "ParkingLotA",
                PlateNumber = "A12345",
                CreateTime = System.DateTime.Now,
                OrderStatus = true,
            };
            var parkingOrderContent = BuildRequestBody(parkingOrder);

            //when
            var response = await client.PostAsync("/parkingOrders", parkingOrderContent);

            //then
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var createdParkingOrder = await DeserializeResponse<ParkingOrderDto>(response);
            Assert.Equivalent(parkingOrder,createdParkingOrder);
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