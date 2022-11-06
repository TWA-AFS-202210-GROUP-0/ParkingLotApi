using Microsoft.EntityFrameworkCore;
using ParkingLotApi.Models;

namespace ParkingLotApi.Repository
{
    public class ParkingContext : DbContext
    {
        public ParkingContext(DbContextOptions<ParkingContext> options)
            : base(options)
        {
        }

        public DbSet<ParkingLotEntity> ParkingLot{ get; set; }
    }
}