using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Moq;
using MySqlConnector;
using ParkingLotApi.Controllers;
using ParkingLotApi.Dto;
using ParkingLotApi.Services;
using Xunit.Abstractions;

namespace ParkingLotApiTest.ControllerTest
{
    public class ParkingLotControllerTest
    {
        [Fact]
        public async Task Should_create_parking_lot_whith_mock_service()
        {
            //Given
            var parkingLotService = new Mock<IParkingLotService>();
            parkingLotService.Setup(m => m.AddParkingLot(It.IsAny<ParkingLotDto>())).ReturnsAsync(1);
            var parkingLotController = new ParkingLotController(parkingLotService.Object);
            //When
            var actionResult = await parkingLotController.CreateParkingLot(new ParkingLotDto()
            {
                Name = "SLB",
                Capacity = 10,
                Location = "TUSPark",
            });
            //Then
            var createdResult = Assert.IsType<CreatedResult>(actionResult.Result);
            var dtoResult = Assert.IsType<ParkingLotDto>(createdResult.Value);
            Assert.Equal("SLB",dtoResult.Name);
            Assert.Equal(201,createdResult.StatusCode);
        }

        [Fact]
        public async Task should_fail_create_parking_lot_when_capacity_is_negative()
        {
            //Given
            var parkingLotService = new Mock<IParkingLotService>();
            parkingLotService.Setup(m => m.AddParkingLot(It.IsAny<ParkingLotDto>())).ReturnsAsync(1);
            var parkingLotController = new ParkingLotController(parkingLotService.Object);
            //When
            var actionResult = await parkingLotController.CreateParkingLot(new ParkingLotDto()
            {
                Name = "SLB",
                Capacity = -1,
                Location = "TUSPark",
            });
            //Then
            Assert.IsType<BadRequestResult>(actionResult.Result);
        }

        [Fact]
        public async Task should_fail_create_parking_lot_when_name_comflict()
        {
            //Given
            var parkingLotService = new Mock<IParkingLotService>();
            parkingLotService.Setup(m => m.AddParkingLot(It.IsAny<ParkingLotDto>())).Throws<DuplicateNameException>();
            var parkingLotController = new ParkingLotController(parkingLotService.Object);
            //When
            var actionResult = await parkingLotController.CreateParkingLot(new ParkingLotDto()
            {
                Name = "SLB",
                Capacity = 10,
                Location = "TUSPark",
            });
            //Then
            Assert.IsType<ConflictResult>(actionResult.Result);
        }

        [Fact]
        public async Task should_delete_parking_lot()
        {
            //Given
            var parkingLotService = new Mock<IParkingLotService>();
            parkingLotService.Setup(m => m.DeleteParkingLot(It.IsAny<int>(),It.IsAny<string>())).ReturnsAsync("SLB");
            var parkingLotController = new ParkingLotController(parkingLotService.Object);
            //when
            var actionResult = await parkingLotController.DeleteParkingLot(1, "SLB");
            //Then
            Assert.IsType(typeof(OkResult),actionResult);
        }


        [Fact]
        public async Task should_fail_delete_parking_lot()
        {
            //Given
            var parkingLotService = new Mock<IParkingLotService>();
            parkingLotService.Setup(m => m.DeleteParkingLot(It.IsAny<int>(), It.IsAny<string>()))
                .Throws<NullReferenceException>();
            var parkingLotController = new ParkingLotController(parkingLotService.Object);
            //when
            var actionResult = await parkingLotController.DeleteParkingLot(1, "SLB");
            //Then
            Assert.IsType(typeof(NotFoundResult), actionResult);
        }

        [Fact]
        public async Task Should_get_parking_lots_by_pageIndex()
        {
            //given
            var parkingLotService = new Mock<IParkingLotService>();
            parkingLotService.Setup(m => m.GetMultiParkingLots(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(
                new List<ParkingLotDto>
                {
                    new ParkingLotDto(){ Name = "SLB", Capacity = 10, Location = "TUSPark", },
                    new ParkingLotDto(){ Name = "SLB2", Capacity = 10, Location = "TUSPark", },
                });
            var parkingLotController = new ParkingLotController(parkingLotService.Object);
            //when
            var actionResult = await parkingLotController.Get15ParkingLotsOnePage(2);
            //Then
            var getResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var dtoResult = Assert.IsType<List<ParkingLotDto>>(getResult.Value);
            Assert.Equal(2, dtoResult.Count);
        }

        [Fact]
        public async Task Should_faile_get_parking_lots_by_pageIndex_given_pageIndex_isLessThan_1()
        {
            //given
            var parkingLotService = new Mock<IParkingLotService>();
            parkingLotService.Setup(m => m.GetMultiParkingLots(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(
                new List<ParkingLotDto>
                {
                    new ParkingLotDto(){ Name = "SLB", Capacity = 10, Location = "TUSPark", },
                    new ParkingLotDto(){ Name = "SLB2", Capacity = 10, Location = "TUSPark", },
                });
            var parkingLotController = new ParkingLotController(parkingLotService.Object);
            //when
            var actionResult = await parkingLotController.Get15ParkingLotsOnePage(0);
            //Then
            Assert.IsType(typeof(BadRequestResult), actionResult.Result);
        }

        [Fact]
        public async Task Should_return_not_found_get_parking_lots_when_out_of_range()
        {
            //given
            var parkingLotService = new Mock<IParkingLotService>();
            parkingLotService.Setup(m => m.GetMultiParkingLots(It.IsAny<int>(), It.IsAny<int>()))
                .Throws<NullReferenceException>();
            var parkingLotController = new ParkingLotController(parkingLotService.Object);
            //when
            var actionResult = await parkingLotController.Get15ParkingLotsOnePage(2);
            //Then
            Assert.IsType(typeof(NotFoundResult), actionResult.Result);
        }
    }
}
