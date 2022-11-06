using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ParkingLotApi.Dtos;

namespace ParkingLotApiTest.ControllerTest
{
    [Collection("1")]
    public class ParkingLotControllerTest : TestBase
    {
        public ParkingLotControllerTest(CustomWebApplicationFactory<Program> factory)
            : base(factory)
        {
        }

        [Fact]
        public async Task Should_create_parkingLot_success()
        {
            // given
            var client = GetClient();
            ParkingLotDto parkingLotDto = new ParkingLotDto
            {
                Name = "SLB",
                Capacity = 10,
                Location = "Tus"
            };
            // when
            var httpContent = JsonConvert.SerializeObject(parkingLotDto);
            var content = new StringContent(httpContent, Encoding.UTF8, MediaTypeNames.Application.Json);
            await client.PostAsync("/parkingLots", content);
            // then
            var allParkingLotsResponse = await client.GetAsync("/parkingLots");
            var body = await allParkingLotsResponse.Content.ReadAsStringAsync();
            var returnParkingLots = JsonConvert.DeserializeObject<List<ParkingLotDto>>(body);

            Assert.Single(returnParkingLots);
        }

        [Fact]
        public async Task Should_create_parkingLot_fail_when_name_is_repeat()
        {
            // given
            var client = GetClient();
            ParkingLotDto parkingLotDto = new ParkingLotDto
            {
                Name = "SLB",
                Capacity = 10,
                Location = "Tus"
            };
            var httpContent = JsonConvert.SerializeObject(parkingLotDto);
            var content = new StringContent(httpContent, Encoding.UTF8, MediaTypeNames.Application.Json);
            await client.PostAsync("/parkingLots", content);
            ParkingLotDto repeatParkingLotDto = new ParkingLotDto
            {
                Name = "SLB",
                Capacity = 11,
                Location = "Tus"
            };
            var repeatHttpContent = JsonConvert.SerializeObject(repeatParkingLotDto);
            var repeatContent = new StringContent(repeatHttpContent, Encoding.UTF8, MediaTypeNames.Application.Json);
            // when
            var response = await client.PostAsync("/parkingLots", repeatContent);
            // then
            Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
        }

        [Fact]
        public async Task Should_create_parkingLot_fail_when_capacity_is_out_of_range()
        {
            // given
            var client = GetClient();
            ParkingLotDto parkingLotDto = new ParkingLotDto
            {
                Name = "SLB",
                Capacity = -1,
                Location = "Tus"
            };
            var httpContent = JsonConvert.SerializeObject(parkingLotDto);
            var content = new StringContent(httpContent, Encoding.UTF8, MediaTypeNames.Application.Json);
            // when
            var response = await client.PostAsync("/parkingLots", content);
            // then
            Assert.Equal(HttpStatusCode.Accepted, response.StatusCode);
        }
    }
}
