using Microsoft.EntityFrameworkCore;
using ParkingLotApi.Repository;
using ParkingLotApi.Services;
using System;

namespace ParkingLotApiTest.ServiceTest
{
    public class ServiceTestBase : IDisposable
    {
        protected readonly ParkingDbContext parkingDbContext;
        protected readonly ParkingLotService parkingLotService;
        protected readonly ParkingOrderService parkingOrderService;

        public ServiceTestBase()
        {
            var options = new DbContextOptionsBuilder<ParkingDbContext>()
                .UseInMemoryDatabase(databaseName: $"{GetType()}")
                .Options;

            parkingDbContext = new ParkingDbContext(options);
            parkingLotService = new ParkingLotService(parkingDbContext);
            parkingOrderService = new ParkingOrderService(parkingDbContext);

        }

        public async void Dispose()
        {
            parkingDbContext.ParkingLots.RemoveRange(parkingDbContext.ParkingLots);
            parkingDbContext.ParkingOrders.RemoveRange(parkingDbContext.ParkingOrders);
            await parkingDbContext.SaveChangesAsync();
        }
    }
}