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

        [Fact]
        public async Task Should_get_parking_lots_by_page()
        {
            // given
            var client = GetClient();
            for (var i = 0; i < 18; i++)
            {
                ParkingLotDTO parkingLotDTO = new ParkingLotDTO()
                {
                    Name = "hi" + i,
                    Capacity = 1,
                    Location = "hihi",
                };
                var requestBody = CreateRequestBody(parkingLotDTO);
                await client.PostAsync("/parkinglots", requestBody);
            }

            // when
            var page2ParkingLotsResponse = await client.GetAsync("/parkinglots?pageNumber=2");

            // then
            var parkingLots = await DeserializeResponse<List<ParkingLotDTO>>(page2ParkingLotsResponse);
            Assert.Equal(3, parkingLots.Count());
        }

        [Fact]
        public async Task Should_get_one_parking_lot_successfully()
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
            var getResponse = await client.GetAsync($"/parkinglots/{id}");

            // then
            var parkingLot = await DeserializeResponse<ParkingLotDTO>(getResponse);
            Assert.Equal(parkingLotDTO.Name, parkingLot.Name);
            Assert.Equal(parkingLotDTO.Capacity, parkingLot.Capacity);
            Assert.Equal(parkingLotDTO.Location, parkingLot.Location);
        }

        [Fact]
        public async Task Should_update_one_parking_lot_capacity_successfully()
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
            requestBody = CreateRequestBody(11);

            //when
            var getResponse = await client.PatchAsync($"/parkinglots/{id}", requestBody);

            // then
            var parkingLot = await DeserializeResponse<ParkingLotDTO>(getResponse);
            Assert.Equal(parkingLotDTO.Name, parkingLot.Name);
            Assert.Equal(11, parkingLot.Capacity);
            Assert.Equal(parkingLotDTO.Location, parkingLot.Location);
        }

        [Fact]
        public async Task Should_park_one_car_in_parking_lot_successfully()
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
            requestBody = CreateRequestBody("XXX111");

            //when
            var getResponse = await client.PostAsync($"/parkinglots/{id}/orders", requestBody);

            // then
            var order = await DeserializeResponse<OrderDTO>(getResponse);
            Assert.Equal("XXX111", order.PlateNumber);
            Assert.Equal(parkingLotDTO.Name, order.ParkingLotName);
            Assert.NotNull(order.CreationTime);
        }

        [Fact]
        public async Task Should_not_park_one_car_in_parking_lot_when_lot_is_at_capacity()
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
            requestBody = CreateRequestBody("XXX111");
            await client.PostAsync($"/parkinglots/{id}/orders", requestBody);
            requestBody = CreateRequestBody("XXX222");

            //when
            var getResponse = await client.PostAsync($"/parkinglots/{id}/orders", requestBody);

            // then
            Assert.Equal(System.Net.HttpStatusCode.Forbidden, getResponse.StatusCode);
            //Assert.Equal("The parking lot is full", getResponse.Content.ToString());
        }

        [Fact]
        public async Task Should_leave_one_car_in_parking_lot_successfully()
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
            requestBody = CreateRequestBody("XXX111");
            var getResponse = await client.PostAsync($"/parkinglots/{id}/orders", requestBody);
            var order = await DeserializeResponse<OrderDTO>(getResponse);
            requestBody = CreateRequestBody(order);

            // when
            response = await client.PutAsync($"/parkinglots/{id}/orders", requestBody);
            var leftOrder = await DeserializeResponse<OrderDTO>(response);

            // then
            Assert.Equal("XXX111", order.PlateNumber);
            Assert.Equal(parkingLotDTO.Name, leftOrder.ParkingLotName);
            Assert.NotNull(leftOrder.CreationTime);
            Assert.NotNull(leftOrder.ClosedTime);
            Assert.Equal("closed", leftOrder.Status);
        }
    }
}