using Microsoft.EntityFrameworkCore;
using ParkingLotApi.Repository;
using ParkingLotApi.Services;
using System;

namespace ParkingLotApiTest.ServiceTest
{
    public class ServiceTestBase : IDisposable
    {
        protected readonly ParkingDbContext ParkingDbContext;
        protected readonly ParkingLotService ParkingLotService;

        public ServiceTestBase()
        {
            var options = new DbContextOptionsBuilder<ParkingDbContext>()
                .UseInMemoryDatabase(databaseName: $"{GetType()}")
                .Options;

            ParkingDbContext = new ParkingDbContext(options);
            ParkingLotService = new ParkingLotService(ParkingDbContext);

        }

        public async void Dispose()
        {
            ParkingDbContext.ParkingLots.RemoveRange(ParkingDbContext.ParkingLots);
            await ParkingDbContext.SaveChangesAsync();
        }
    }
}