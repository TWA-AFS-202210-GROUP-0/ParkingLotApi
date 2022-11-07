using Microsoft.Extensions.DependencyInjection;
using ParkingLotApi.Dtos;
using ParkingLotApi.Exceptions;
using ParkingLotApi.Repository;
using ParkingLotApi.Services;
using System.Threading.Tasks;

namespace ParkingLotApiTest.ServiceTest
{
    [Collection("Sequential")]
    public class ParkingLotServiceTest: TestBase
    {
        public ParkingLotServiceTest(CustomWebApplicationFactory<Program> factory) : base(factory)
        {
        }

        [Fact]
        public async Task Should_throw_exception_when_add_parking_lot_again()
        {
            // given
            var context = GetParkingLotContext();
            var parkingLot = new ParkingLotDto
            {
                Name = "ParkingLotA",
                Capacity = 50,
                Location = "Street A",
            };
            ParkingLotService parkingLotService = new ParkingLotService(context);
            await parkingLotService.AddParkingLot(parkingLot);

            // when
            // then
            await Assert.ThrowsAsync<WrongNameException>(() => parkingLotService.AddParkingLot(parkingLot));

        }

        [Fact]
        public async Task Should_throw_exception_when_add_parking_lot_with_minus_capacity()
        {
            // given
            var context = GetParkingLotContext();
            var parkingLot = new ParkingLotDto
            {
                Name = "ParkingLotA",
                Capacity = -10,
                Location = "Street A",
            };
            ParkingLotService parkingLotService = new ParkingLotService(context);

            // when
            // then
            await Assert.ThrowsAsync<WrongCapacityException>(() => parkingLotService.AddParkingLot(parkingLot));

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

