using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ParkingLotApi.Controllers;
using ParkingLotApi.Dto;
using ParkingLotApi.Services;

namespace ParkingLotApiTest.ControllerTest
{
    public class ParkingOrderControllerTest
    {

        [Fact]
        public async Task should_create_parking_order()
        {
            //given
            var parkingOrderService = new Mock<IParkingOrderService>();
            parkingOrderService.Setup(x => x.CreateOrder(It.IsAny<int>(), It.IsAny<ParkingOrderDto>())).ReturnsAsync(
                new ParkingOrderDto()
                {
                    Id = 1,
                    PlateNumber = "12345",
                    NameOfParkingLot = "SLB",
                    CreateTime = DateTime.Now,
                    IsOpen = true
                });
            var parkingOrderController = new ParkingOrderController(parkingOrderService.Object);
            //When
            var actionResult = await parkingOrderController.PostOrder(1, new ParkingOrderDto());
            //Then
            var postResult = Assert.IsType<CreatedResult>(actionResult.Result);
            var dtoResult = Assert.IsType<ParkingOrderDto>(postResult.Value);
            Assert.Equal("12345", dtoResult.PlateNumber);
        }

        [Fact]
        public async Task should_not_create_parking_order_when_is_full()
        {
            //given
            var parkingOrderService = new Mock<IParkingOrderService>();
            parkingOrderService.Setup(x => x.CreateOrder(It.IsAny<int>(), It.IsAny<ParkingOrderDto>()))
                .Throws(new NullReferenceException("The parking lot is full."));
                var parkingOrderController = new ParkingOrderController(parkingOrderService.Object);
            //When
            var actionResult = await parkingOrderController.PostOrder(1, new ParkingOrderDto());
            //then
            var postResult = Assert.IsType<NotFoundObjectResult>(actionResult.Result);
            var result = Assert.IsType<string>(postResult.Value);
            Assert.Equal("The parking lot is full.", result);
        }

        [Fact]
        public async Task Should_change_status_to_closed_when_drive_out()
        {
            //given
            var parkingOrderService = new Mock<IParkingOrderService>();
            parkingOrderService.Setup(x => x.CloseOrder(It.IsAny<int>(), It.IsAny<ParkingOrderDto>()))
                .ReturnsAsync(new ParkingOrderDto()
                {
                    PlateNumber = "12345",
                    CloseTime = DateTime.Now,
                    IsOpen = false,
                });
            var parkingOrderController = new ParkingOrderController(parkingOrderService.Object);
            //when
            var actionResult = await parkingOrderController.UpdateOrder(1, new ParkingOrderDto() { });
            var putResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var dtoResult = Assert.IsType<ParkingOrderDto>(putResult.Value);
            Assert.False(dtoResult.IsOpen);
        }

    }
}
