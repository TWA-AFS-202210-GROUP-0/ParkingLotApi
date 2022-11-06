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

        [Fact]
        public async Task Should_get_all_parkingLots_seccessfully_via_parkingLot_service()
        {
            // given
            var context = GetParkingLotDbContext();
            var parkingLotDto = new ParkingLotDto
            {
                Name = "SLB",
                Capacity = 10,
                Location = "Tus"
            };
            var parkingLotDto1 = new ParkingLotDto
            {
                Name = "TW",
                Capacity = 10,
                Location = "Tus"
            };
            ParkingLotService parkingLotService = new ParkingLotService(context);
            parkingLotService.AddParkingLot(parkingLotDto);
            parkingLotService.AddParkingLot(parkingLotDto1);
            // when
            var parkingLotDtos = parkingLotService.GetAll();
            // then
            Assert.Equal(2, parkingLotDtos.Count());
        }

        [Fact]
        public void Should_get_a_parkingLot_by_id_success()
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
            var id = parkingLotService.AddParkingLot(parkingLotDto);
            // when
            var parkingLotDtoWithSpecialId = parkingLotService.GetParkingLotById(id);
            // then
            Assert.Equal(parkingLotDto.Name, parkingLotDtoWithSpecialId.Name);
        }

        [Fact]
        public void Should_delete_a_parkingLot_by_id_success()
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
            var id = parkingLotService.AddParkingLot(parkingLotDto);
            // when
            var response = parkingLotService.DeleteParkingLotById(id);
            var foundResult = parkingLotService.GetParkingLotById(id);
            // then
            Assert.Equal(parkingLotDto.Name, response.Name);
            Assert.Null(foundResult);
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
