using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ParkingLotApi.Models
{
    [Index(nameof(Name), IsUnique = true)]
    public class ParkingLotEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Capacity { get; set; }

        public string Location { get; set; }
    }
}
