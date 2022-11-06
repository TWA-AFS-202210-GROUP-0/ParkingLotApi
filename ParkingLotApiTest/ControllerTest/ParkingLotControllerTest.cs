using System.Net;
using System.Threading.Tasks;
using ParkingLotApi;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using ParkingLotApi.Dtos;
using Newtonsoft.Json;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Net.Http;
using System.Text;
using System.Collections.Generic;

namespace ParkingLotApiTest.ControllerTest
{
    [Collection("aaa")]
    public class ParkingLotControllerTest : TestBase
    {
        public ParkingLotControllerTest(CustomizedWebApplication<Program> factory) : base(factory)
        {
        }


        [Fact]
        public async Task Should_add_parking_lot_success()
        {
            // given
            var client = GetClient();
            ParkingLotDto parkingLot = new ParkingLotDto
            {
                Capacity = 10,
                Location = "536 South Forest Avenue",
                Name = "South Forest"
            };
            // when
            var response = await client.PostAsync("ParkingLots", SerializedObject(parkingLot));
            var createdParkingLot = await ParseObject<ParkingLotDto>(response);
            // then
            Assert.Equivalent(parkingLot, createdParkingLot);
        }

        [Fact]
        public async Task Should_add_parking_lot_FAIL_when_capacity_minus()
        {
            // given
            var client = GetClient();
            ParkingLotDto parkingLot = new ParkingLotDto
            {
                Capacity = -20,
                Location = "536 South Forest Avenue",
                Name = "South Forest"
            };
            // when
            var response = await client.PostAsync("ParkingLots", SerializedObject(parkingLot));
            // then
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Should_add_parking_lot_FAIL_when_name_has_existed()
        {
            // given
            var client = GetClient();
            ParkingLotDto parkingLot = new ParkingLotDto
            {
                Capacity = 10,
                Location = "536 South Forest Avenue",
                Name = "South Forest"
            };
            ParkingLotDto parkingLot2 = new ParkingLotDto
            {
                Capacity = 10,
                Location = "536 South Forest Avenue",
                Name = "South Forest"
            };
            // when
            await client.PostAsync("ParkingLots", SerializedObject(parkingLot));
            var response = await client.PostAsync("ParkingLots", SerializedObject(parkingLot2));
            // then
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Should_delete_parking_lot_success()
        {
            // given
            var client = GetClient();
            ParkingLotDto parkingLot = new ParkingLotDto
            {
                Capacity = 10,
                Location = "536 South Forest Avenue",
                Name = "South Forest"
            };
            // when
            var response = await client.PostAsync("ParkingLots", SerializedObject(parkingLot));
            await client.DeleteAsync(response.Headers.Location);
            var allParkingLotsresponse = await client.GetAsync("ParkingLots");
            var parkinglots = await ParseObject<List<ParkingLotDto>>(allParkingLotsresponse);
            // then
            response.EnsureSuccessStatusCode();
            Assert.Empty(parkinglots);
        }


        public async Task<T> ParseObject<T>(HttpResponseMessage response)
        {
            var responseBody = await response.Content.ReadAsStringAsync();
            var createdObject = JsonConvert.DeserializeObject<T>(responseBody);
            return createdObject;
        }

        public StringContent SerializedObject(object obj)
        {
            var serializedObjectBody = JsonConvert.SerializeObject(obj);
            var postBodyOne = new StringContent(serializedObjectBody, Encoding.UTF8, "application/json");
            return postBodyOne;
        }

        public async Task<ParkingLotDto> CreateParkinglotForTest(HttpClient httpClient, string location, string name, int capacity)
        {
            var parkingLot = new ParkingLotDto();
            parkingLot.Capacity = capacity;
            parkingLot.Name = name;
            parkingLot.Location = location;
            var postBody = SerializedObject(parkingLot);
            var response = await httpClient.PostAsync("ParkingLots", postBody);
            var result = await ParseObject<ParkingLotDto>(response);
            return result;
        }

    }
}