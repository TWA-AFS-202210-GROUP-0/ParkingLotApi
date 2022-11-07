using Microsoft.Extensions.DependencyInjection;
using ParkingLotApi.Repository;
using System;
using System.Net.Http;

namespace ParkingLotApiTest
{
    public class TestBase : IClassFixture<CustomWebApplicationFactory<Program>>, IDisposable
    {
        public TestBase(CustomWebApplicationFactory<Program> factory)
        {
            this.Factory = factory;
        }

        protected CustomWebApplicationFactory<Program> Factory { get; }

        public void Dispose()
        {
            var scope = Factory.Services.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var context = scopedServices.GetRequiredService<ParkingLotContext>();

            //context.profileEntities.RemoveRange(context.profileEntities);
            //context.employeeEntities.RemoveRange(context.employeeEntities);
            //context.CompanyEntities.RemoveRange(context.CompanyEntities);
            context.ParkingLots.RemoveRange(context.ParkingLots);

            context.SaveChanges();
        }

        protected HttpClient GetClient()
        {
            return Factory.CreateClient();
        }
    }
}