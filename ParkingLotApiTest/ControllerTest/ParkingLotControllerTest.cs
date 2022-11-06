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

        [Fact]
        public async Task Should_get_parkingLot_by_id_success()
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
            var response = await client.PostAsync("/parkingLots", content);
            var body = await response.Content.ReadAsStringAsync();
            var id = JsonConvert.DeserializeObject<int>(body);
            // when
            var responsePosted = await client.GetAsync($"parkingLots/{id}");
            // then
            var bodyPosted = await responsePosted.Content.ReadAsStringAsync();
            var parkingLotDtoPosted = JsonConvert.DeserializeObject<ParkingLotDto>(bodyPosted);
            Assert.Equal(parkingLotDto.Name, parkingLotDtoPosted.Name);
        }

        [Fact]
        public async Task Should_delete_parkingLot_by_id_success()
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
            var response = await client.PostAsync("/parkingLots", content);
            var body = await response.Content.ReadAsStringAsync();
            var id = JsonConvert.DeserializeObject<int>(body);
            // when
            var responseDeleted = await client.DeleteAsync($"parkingLots/{id}");
            // then
            var bodyDeleted = await responseDeleted.Content.ReadAsStringAsync();
            var parkingLotDtoDeleted = JsonConvert.DeserializeObject<ParkingLotDto>(bodyDeleted);
            Assert.Equal(parkingLotDto.Name, parkingLotDtoDeleted.Name);

            var responsePosted = await client.GetAsync($"parkingLots/{id}");
            var bodyPosted = await responsePosted.Content.ReadAsStringAsync();
            var parkingLotDtoPosted = JsonConvert.DeserializeObject<ParkingLotDto>(bodyPosted);
            Assert.Equal(HttpStatusCode.NotFound, responsePosted.StatusCode);
        }

        [Fact]
        public async Task Should_get_parkingLot_of_one_page_success()
        {
            // given
            var client = GetClient();
            var parkingLotDtosPosted = await AddTestParkingLots(client);
            // when
            int page = 2;
            var response = await client.GetAsync($"parkingLots/byPage?page={page}");
            // then
            var body = await response.Content.ReadAsStringAsync();
            var parkingLotDtosGotton = JsonConvert.DeserializeObject<List<ParkingLotDto>>(body);
            Assert.Equal(parkingLotDtosPosted[15].Name, parkingLotDtosGotton[0].Name);
        }

        private async Task<List<ParkingLotDto>> AddTestParkingLots(HttpClient client)
        {
            var parkingLotDtos = new List<ParkingLotDto>();
            parkingLotDtos.Add(generateParkingLotDto("SLB", 10, "TUS"));
            for (int i = 0; i < 14; i++)
            {
                parkingLotDtos.Add(generateParkingLotDto($"TW{i}", 10, $"TUS{i}"));
            }
            parkingLotDtos.Add(generateParkingLotDto("Intel", 30, "PKU"));
            foreach (var parkingLotDto in parkingLotDtos)
            {
                var httpContent = JsonConvert.SerializeObject(parkingLotDto);
                var content = new StringContent(httpContent, Encoding.UTF8, MediaTypeNames.Application.Json);
                await client.PostAsync("/parkingLots", content);
            }

            return parkingLotDtos;
        }

        private ParkingLotDto generateParkingLotDto(string name, int capacity, string location)
        {
            return new ParkingLotDto
            {
                Name = name,
                Capacity = capacity,
                Location = location
            };
        }
    }
}
