using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ParkingLotApi.Dtos;

namespace ParkingLotApiTest.ControllerTest
{
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
    }
}
