using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using ParkingLotApi.Dtos;
using ParkingLotApi.Repository;
using ParkingLotApi.Services;

namespace ParkingLotApiTest.ServiceTest
{
    [Collection("1")]
    public class ParkingLotServiceTest : TestBase
    {
        public ParkingLotServiceTest(CustomWebApplicationFactory<Program> factory)
            : base(factory) { }

        [Fact]
        public async Task Should_create_parkingLot_seccessfully_via_parkingLot_service()
        {
            // given
            var context = GetParkingLotDbContext();
            var parkingLotDto = new ParkingLotDto
            {
                Name = "SLB",
                Capacity = 10,
                Location = "Tus"
            };
            ParkingLotService parkingLotService = new ParkingLotService(context);
            // when
            parkingLotService.AddParkingLot(parkingLotDto);
            // then
            Assert.Equal(1, context.ParkingLots.Count());
        }

        private ParkingLotDbContext GetParkingLotDbContext()
        {
            var scope = Factory.Services.CreateScope();
            var scopeService = scope.ServiceProvider;
            ParkingLotDbContext context = scopeService.GetRequiredService<ParkingLotDbContext>();
            return context;
        }
    }
}
