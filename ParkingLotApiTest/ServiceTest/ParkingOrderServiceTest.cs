using Microsoft.Extensions.DependencyInjection;
using ParkingLotApi.Dtos;
using ParkingLotApi.Exceptions;
using ParkingLotApi.Repository;
using ParkingLotApi.Services;
using System.Threading.Tasks;

namespace ParkingLotApiTest.ServiceTest
{
    [Collection("Sequential")]
    public class ParkingOrderServiceTest: TestBase
    {
        public ParkingOrderServiceTest(CustomWebApplicationFactory<Program> factory) : base(factory)
        {
        }

        [Fact]
        public async Task Should_throw_exception_when_parking_given_a_full_parking_lot()
        {
            // given
            var context = GetParkingLotContext();
            var parkingLot = new ParkingLotDto
            {
                Name = "ParkingLotA",
                Capacity = 1,
                Location = "Street A",
            };
            ParkingLotService parkingLotService = new ParkingLotService(context);
            await parkingLotService.AddParkingLot(parkingLot);
            var parkingOrder = new ParkingOrderDto
            {
                ParkingLotName = "ParkingLotA",
                PlateNumber = "A12345",
                CreateTime = System.DateTime.Now,
                OrderStatus = true,
            };
            ParkingOrderService parkingOrderService = new ParkingOrderService(context);
            await parkingOrderService.AddParkingOrder(parkingOrder);
            var parkingOrder2 = new ParkingOrderDto
            {
                ParkingLotName = "ParkingLotA",
                PlateNumber = "B12345",
                CreateTime = System.DateTime.Now,
                OrderStatus = true,
            };
            // when
            // then
            var response = await Assert.ThrowsAsync<NoSpaceException>(() => parkingOrderService.AddParkingOrder(parkingOrder2));
            Assert.Equal("The parking lot is full", response.Message);
        }

        private ParkingLotContext GetParkingLotContext()
        {
            var scope = Factory.Services.CreateScope();
            var scopedService = scope.ServiceProvider;
            ParkingLotContext context = scopedService.GetRequiredService<ParkingLotContext>();
            return context;
        }
    }
}

