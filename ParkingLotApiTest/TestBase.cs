using ParkingLotApi.Repository;
using Microsoft.Extensions.DependencyInjection;
using ParkingLotApiTest;
using System.Net.Http;
using System;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Text;

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

            context.ParkingLots.RemoveRange(context.ParkingLots);
            context.Orders.RemoveRange(context.Orders);
            context.SaveChanges();
        }

        protected HttpClient GetClient()
        {
            return Factory.CreateClient();
        }

        protected StringContent CreateRequestBody(object obj)
        {
            var serializeObject = JsonConvert.SerializeObject(obj);
            return new StringContent(serializeObject, Encoding.UTF8, "application/json");
        }

        protected async Task<T> DeserializeResponse<T>(HttpResponseMessage response)
        {
            var responseBody = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(responseBody);
        }
    }
}