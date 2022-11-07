using Microsoft.EntityFrameworkCore;

namespace ParkingLotApi.Repository
{
    public class ParkingLotContext : DbContext
    {
        public ParkingLotContext(DbContextOptions<ParkingLotContext> options)
            : base(options)
        {
        }

        public DbSet<ParkingLot>ParkingLots { get; set; }

        public DbSet<Order> Orders { get; set; }
    }
}