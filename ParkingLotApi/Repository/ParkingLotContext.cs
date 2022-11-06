using Microsoft.EntityFrameworkCore;
using ParkingLotApi.Models;

namespace ParkingLotApi.Repository
{
    public class ParkingDbContext : DbContext
    {
        public ParkingDbContext(DbContextOptions<ParkingDbContext> options)
            : base(options)
        {
        }

        public DbSet<ParkingLotEntity> ParkingLots{ get; set; }
        public DbSet<ParkingOrderEntity> ParkingOrders { get; set; }
    }
}