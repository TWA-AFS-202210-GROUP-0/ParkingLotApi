using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using ParkingLotApi.Repository;

namespace ParkingLotApiTest
{
    public class TestBase : IClassFixture<CustomizedWebApplication<Program>>, IDisposable
    {
        public TestBase(CustomizedWebApplication<Program> factory)
        {
            this.Factory = factory;
        }

        protected CustomizedWebApplication<Program> Factory { get; }

        public void Dispose()
        {
            var scope = Factory.Services.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var context = scopedServices.GetRequiredService<ParkingLotContext>();

            context.ParkingLots.RemoveRange(context.ParkingLots);
            context.SaveChanges();
        }

        protected HttpClient GetClient()
        {
            return Factory.CreateClient();
        }
    }
}
