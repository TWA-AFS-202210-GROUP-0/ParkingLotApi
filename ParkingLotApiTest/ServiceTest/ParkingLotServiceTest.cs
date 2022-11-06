using Microsoft.Extensions.DependencyInjection;
using ParkingLotApi.Dto;
using ParkingLotApi.Repository;
using ParkingLotApi.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingLotApiTest.ServiceTest
{
    [Collection("Our Test Collection #1")]
    public class ParkingLotServiceTest : TestBase
    {
        public ParkingLotServiceTest(CustomWebApplicationFactory<Program> factory) : base(factory)
        {
        }

        [Fact]
        public async Task Should_create_company_success_via_company_service()
        {
            //given
            var context = GetCompanyDbContext();
            ParkingLotDto companyDto = new ParkingLotDto
            {
                Name = "IBM",
                Capacity = 10,
                Location = "xx"
            };
            parkingLotService companyService = new parkingLotService(context);
            // when
            await companyService.AddOrUpdateCompany(companyDto, true);

            // then
            Assert.Equal(1, context.ParkingLots.Count());
        }

        [Fact]
        public async Task Should_get_all_company_success_via_company_service()
        {
            //given
            var context = GetCompanyDbContext();
            var c1 = getCompanyDto();
            var c2 = getCompanyDto();
            c2.Name = "IMB";
            parkingLotService companyService = new parkingLotService(context);
            await companyService.AddOrUpdateCompany(c1, true);
            await companyService.AddOrUpdateCompany(c2, true);
            // when
            var res = await companyService.GetAll();

            // then
            Assert.Equal(2, res.Count());
        }
        [Fact]
        public async Task Should_get_company_success_by_Id_via_company_service()
        {
            //given
            var context = GetCompanyDbContext();
            var c1 = getCompanyDto();
            parkingLotService companyService = new parkingLotService(context);
            var res = await companyService.AddOrUpdateCompany(c1, true);
            // when
            var newRes = await companyService.GetById(res);

            // then
            Assert.Equal("IBM", newRes.Name);
        }


        private ParkingLotContext GetCompanyDbContext()
        {
            var scope = Factory.Services.CreateScope();
            var scopeService  = scope.ServiceProvider;
            ParkingLotContext context = scopeService.GetRequiredService<ParkingLotContext>();
            return context;
        }

        private ParkingLotDto getCompanyDto()
        {
            ParkingLotDto companyDto = new ParkingLotDto
            {
                Name = "IBM",
                Capacity = 10,
                Location = "xx"
            };
            return companyDto;
        }
    }
}
