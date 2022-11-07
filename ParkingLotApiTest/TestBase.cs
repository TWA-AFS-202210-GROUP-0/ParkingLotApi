using System;
using ParkingLotApi.Repository;
using Microsoft.Extensions.DependencyInjection;
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
            var context = scopedServices.GetRequiredService<ParkingLotDbContext>();

            context.ParkingLots.RemoveRange(context.ParkingLots);

            context.SaveChanges();
        }

        protected HttpClient GetClient()
        {
            return Factory.CreateClient();
        }
    }
}
