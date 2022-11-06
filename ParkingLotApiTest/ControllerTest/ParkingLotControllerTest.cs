using System;
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
using ParkingLotApi.Models;

namespace ParkingLotApiTest.ControllerTest
{
    [Collection("aaa")]
    public class ParkingLotControllerTest : TestBase
    {
        public ParkingLotControllerTest(CustomizedWebApplication<Program> factory) : base(factory)
        {
        }

        ParkingLotDto parkingLot = new ParkingLotDto
        {
            Capacity = 10,
            Location = "536 South Forest Avenue",
            Name = "South Forest"
        };

        ParkingLotDto parkingLotMd = new ParkingLotDto
        {
            Capacity = 20,
            Location = "536 South Forest Avenue",
            Name = "South Forest"
        };

        ParkingLotDto parkingLotwithOrder = new ParkingLotDto
        {
            Capacity = 10,
            Location = "536 South Forest Avenue",
            Name = "South Forest",
            Availibility = 9,
            ParkingOrderDto = new List<ParkingOrderDto>()
            {
                new ParkingOrderDto()
                {
                    CarPlateNumber = "AUCCD",
                    CreateTime = DateTime.Now,
                    NameOfParkingLot = "South Forest",
                    Number = 1,
                    OrderStatus = OrderStatus.OPEN
                },
            },
        };

        ParkingLotDto parkingLotwithClosedOrderBefore = new ParkingLotDto
        {
            Capacity = 10,
            Location = "536 South Forest Avenue",
            Name = "South Forest",
            Availibility = 8,
            ParkingOrderDto = new List<ParkingOrderDto>()
            {
                new ParkingOrderDto()
                {
                    CarPlateNumber = "AUCCD",
                    CreateTime = DateTime.Now,
                    NameOfParkingLot = "South Forest",
                    Number = 1,
                    OrderStatus = OrderStatus.OPEN
                },

                new ParkingOrderDto()
                {
                    CarPlateNumber = "WPLKS",
                    CreateTime = new DateTime(2022, 11, 1, 7, 0, 0),
                    NameOfParkingLot = "South Forest",
                    Number = 2,
                    OrderStatus = OrderStatus.OPEN,
                },
            },
        };

        ParkingLotDto parkingLotwithClosedOrderAfter = new ParkingLotDto
        {
            Capacity = 10,
            Location = "536 South Forest Avenue",
            Name = "South Forest",
            Availibility = 9,
            ParkingOrderDto = new List<ParkingOrderDto>()
            {
                new ParkingOrderDto()
                {
                    CarPlateNumber = "AUCCD",
                    CreateTime = DateTime.Now,
                    NameOfParkingLot = "South Forest",
                    Number = 1,
                    OrderStatus = OrderStatus.OPEN
                },

                new ParkingOrderDto()
                {
                    CarPlateNumber = "WPLKS",
                    CreateTime = new DateTime(2022, 11, 1, 7, 0, 0),
                    NameOfParkingLot = "South Forest",
                    Number = 2,
                    OrderStatus = OrderStatus.CLOSE,
                    CloseTime = DateTime.Now
                },
            },
        };

        ParkingLotDto parkingLotwithFull = new ParkingLotDto
        {
            Capacity = 1,
            Location = "536 South Forest Avenue",
            Name = "South Forest",
        };
        ParkingLotDto parkingLotwithFullBefore = new ParkingLotDto
        {
            Capacity = 1,
            Location = "536 South Forest Avenue",
            Name = "South Forest",
            Availibility = 0,
            ParkingOrderDto = new List<ParkingOrderDto>()
            {
                new ParkingOrderDto()
                {
                    CarPlateNumber = "WPLKS",
                    CreateTime = new DateTime(2022, 11, 1, 7, 0, 0),
                    NameOfParkingLot = "South Forest",
                    Number = 1,
                    OrderStatus = OrderStatus.OPEN,
                },
            },
        };
        ParkingLotDto parkingLotwithFullAfter = new ParkingLotDto
        {
            Capacity = 1,
            Location = "536 South Forest Avenue",
            Name = "South Forest",
            Availibility = 0,
            ParkingOrderDto = new List<ParkingOrderDto>()
            {
                new ParkingOrderDto()
                {
                    CarPlateNumber = "AUCCD",
                    CreateTime = DateTime.Now,
                    NameOfParkingLot = "South Forest",
                    Number = 2,
                    OrderStatus = OrderStatus.OPEN
                },

                new ParkingOrderDto()
                {
                    CarPlateNumber = "WPLKS",
                    CreateTime = new DateTime(2022, 11, 1, 7, 0, 0),
                    NameOfParkingLot = "South Forest",
                    Number = 1,
                    OrderStatus = OrderStatus.OPEN,
                },
            },
        };

        ParkingLotDto parkingLot2 = new ParkingLotDto
        {
            Capacity = 20,
            Location = "55 Paul Avenue",
            Name = "Paul"
        };

        ParkingLotDto parkingLot3 = new ParkingLotDto
        {
            Capacity = 30,
            Location = "12306 D Avenue",
            Name = "D"
        };

        [Fact]
        public async Task Should_add_parking_lot_success()
        {
            // given
            var client = GetClient();
            // when
            var response = await client.PostAsync("ParkingLots", SerializedObject(parkingLot));
            var createdParkingLot = await ParseObject<ParkingLotDto>(response);
            // then
            Assert.Equivalent(parkingLot, createdParkingLot);
        }

        [Fact]
        public async Task Should_add_Order_to_a_parking_lot_success()
        {
            // given
            var client = GetClient();
            // when
            var responsePost = await client.PostAsync("ParkingLots", SerializedObject(parkingLot));
            var url = responsePost.Headers.Location + "/orders";
            var response = await client.PutAsync(url, SerializedObject(parkingLotwithOrder));
            var createdParkingLot = await ParseObject<ParkingLotDto>(response);
            // then
            Assert.Equivalent(parkingLotwithOrder, createdParkingLot);
        }

        [Fact]
        public async Task Should_add_closed_Order_to_a_parking_lot_success()
        {
            // given
            var client = GetClient();
            // when
            var responsePost = await client.PostAsync("ParkingLots", SerializedObject(parkingLot));
            var url = responsePost.Headers.Location + "/orders";
            var responsePut = await client.PutAsync(url, SerializedObject(parkingLotwithClosedOrderBefore));
            var response = await client.PutAsync(url, SerializedObject(parkingLotwithClosedOrderAfter));
            var createdParkingLot = await ParseObject<ParkingLotDto>(response);
            // then
            Assert.Equivalent(parkingLotwithClosedOrderAfter, createdParkingLot);
        }

        [Fact]
        public async Task Should_not_add_Order_to_a_parking_lot_success()
        {
            // given
            var client = GetClient();
            // when
            var responsePost = await client.PostAsync("ParkingLots", SerializedObject(parkingLotwithFull));
            var url = responsePost.Headers.Location + "/orders";
            var responsePut = await client.PutAsync(url, SerializedObject(parkingLotwithFullBefore));
            var response = await client.PutAsync(url, SerializedObject(parkingLotwithFullAfter));
            var createdParkingLot = await ParseObject<ParkingLotDto>(response);
            // then
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        }

        [Fact]
        public async Task Should_add_parking_lot_FAIL_when_capacity_minus()
        {
            // given
            var client = GetClient();
            ParkingLotDto parkingLot = new ParkingLotDto
            {
                Capacity = -10,
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
            // when
            await client.PostAsync("ParkingLots", SerializedObject(parkingLot));
            var response = await client.PostAsync("ParkingLots", SerializedObject(parkingLot));
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
            var responsePost = await client.PostAsync("ParkingLots", SerializedObject(parkingLot));
            var response = await client.DeleteAsync(responsePost.Headers.Location);
            var allParkingLotsresponse = await client.GetAsync("ParkingLots");
            var parkinglots = await ParseObject<List<ParkingLotDto>>(allParkingLotsresponse);
            // then
            Assert.Equal(HttpStatusCode.NoContent,response.StatusCode);
            Assert.Empty(parkinglots);
        }

        [Fact]
        public async Task Should_query_all_parkinglots_in_pagesize_15()
        {
            // given
            var client = GetClient();
            // when
            await client.PostAsync("ParkingLots", SerializedObject(parkingLot));
            await client.PostAsync("ParkingLots", SerializedObject(parkingLot2));
            await client.PostAsync("ParkingLots", SerializedObject(parkingLot3));
            var allParkingLotsresponse = await client.GetAsync("ParkingLots?pageIndex=1");
            var parkinglots = await ParseObject<List<ParkingLotDto>>(allParkingLotsresponse);
            // then
            Assert.Equal(HttpStatusCode.OK, allParkingLotsresponse.StatusCode);
            Assert.Equal(3, parkinglots.Count);
        }

        [Fact]
        public async Task Should_get_specific_existing_parkingLot_successfully()
        {
            // given
            var client = GetClient();
            // when
            var response = await client.PostAsync("ParkingLots", SerializedObject(parkingLot));
            await client.PostAsync("ParkingLots", SerializedObject(parkingLot2));
            await client.PostAsync("ParkingLots", SerializedObject(parkingLot3));
            var parkinglotResponse = await client.GetAsync(response.Headers.Location);
            var queriedParkingLot = await ParseObject<ParkingLotDto>(parkinglotResponse);
            // then
            Assert.Equal(HttpStatusCode.OK, parkinglotResponse.StatusCode);
            Assert.Equivalent(parkingLot, queriedParkingLot);
        }

        [Fact]
        public async Task Should_update_basic_info_parkingLot_successfully()
        {
            // given
            var client = GetClient();
            // when
            var responsePost = await client.PostAsync("ParkingLots", SerializedObject(parkingLot));
            var response = await client.PutAsync(responsePost.Headers.Location, SerializedObject(parkingLotMd));
            var modifiedParkingLot = await ParseObject<ParkingLotDto>(response);
            // then
            Assert.Equivalent(parkingLotMd, modifiedParkingLot);
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