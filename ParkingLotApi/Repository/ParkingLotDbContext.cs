using Microsoft.EntityFrameworkCore;
using ParkingLotApi.Models;

namespace ParkingLotApi.Repository
{
    public class ParkingLotDbContext : DbContext
    {
        public ParkingLotDbContext(DbContextOptions<ParkingLotDbContext> options)
            : base(options)
        {
        }

        public DbSet<ParkingLotEntity> ParkingLots
        {
            get;
            set;
        }

        public DbSet<OrderEntity> Orders
        {
            get; 
            set;
        }
    }
}