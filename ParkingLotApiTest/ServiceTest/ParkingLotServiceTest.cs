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
            var parkingLotDtos = await parkingLotService.GetAll();
            // then
            Assert.Equal(2, parkingLotDtos.Count());
        }

        [Fact]
        public async Task Should_get_a_parkingLot_by_id_success()
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
            var id = await parkingLotService.AddParkingLot(parkingLotDto);
            // when
            var parkingLotDtoWithSpecialId = await parkingLotService.GetParkingLotById(id);
            // then
            Assert.Equal(parkingLotDto.Name, parkingLotDtoWithSpecialId.Name);
        }

        [Fact]
        public async Task Should_delete_a_parkingLot_by_id_success()
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
            var id = await parkingLotService.AddParkingLot(parkingLotDto);
            // when
            var response = await parkingLotService.DeleteParkingLotById(id);
            var foundResult = await parkingLotService.GetParkingLotById(id);
            // then
            Assert.Equal(parkingLotDto.Name, response.Name);
            Assert.Null(foundResult);
        }

        [Fact(Skip = "To do")]
        public async Task Should_get_parkingLot_of_one_page_success()
        {
            // given
            var context = GetParkingLotDbContext();
            ParkingLotService parkingLotService = new ParkingLotService(context);
            var parkingLotDtos = AddTestParkingLots(parkingLotService);
            // when
            int page = 2;
            var response = await parkingLotService.GetParkingLotByPage(page);
            // then
            Assert.Equal(parkingLotDtos[15].Name, response[0].Name);
            Assert.Equal(1, response.Count());
        }

        [Fact]
        public async Task Should_update_parkingLot_capacity_by_id_seccessfully_via_parkingLot_service()
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
            var id = await parkingLotService.AddParkingLot(parkingLotDto);
            var newCapacity = 11;
            // when
            var updateParkingLotCapacityReturn = await parkingLotService.UpdateParkingLotCapacity(id, newCapacity);
            // then
            Assert.Equal(newCapacity, updateParkingLotCapacityReturn.Capacity);
            var parkingLotDtoUpdated = await parkingLotService.GetParkingLotById(id);
            Assert.Equal(newCapacity, parkingLotDtoUpdated.Capacity);
        }

        private List<ParkingLotDto> AddTestParkingLots(ParkingLotService parkingLotService)
        {
            List<ParkingLotDto> parkingLotDtos = new List<ParkingLotDto>();
            parkingLotDtos.Add(generateParkingLotDto("SLB", 10, "TUS"));
            for (int i = 0; i < 14; i++)
            {
                parkingLotDtos.Add(generateParkingLotDto($"TW{i}", 10, $"TUS{i}"));
            }
            parkingLotDtos.Add(generateParkingLotDto("Intel", 30, "PKU"));

            var parkingLotEntities = parkingLotDtos.Select(_ => _.ToEntity()).ToList();
            foreach (var parkingLotDto in parkingLotDtos)
            {
                parkingLotService.AddParkingLot(parkingLotDto);
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

        private ParkingLotDbContext GetParkingLotDbContext()
        {
            var scope = Factory.Services.CreateScope();
            var scopeService = scope.ServiceProvider;
            ParkingLotDbContext context = scopeService.GetRequiredService<ParkingLotDbContext>();
            return context;
        }
    }
}
