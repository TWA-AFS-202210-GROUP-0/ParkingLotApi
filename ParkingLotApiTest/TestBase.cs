using ParkingLotApi.Repository;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using System;

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

            //context.Employees.RemoveRange(context.Employees);
            //context.Companies.RemoveRange(context.Companies);
            //context.Profiles.RemoveRange(context.Profiles);
            context.SaveChanges();
        }

        protected HttpClient GetClient()
        {
            return Factory.CreateClient();
        }
    }
}