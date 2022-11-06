namespace ParkingLotApi.Models
{
    public class ParkingLotEntity
    {
        // public ParkingLotEntity(string name, int capacity, string location)
        // {
        //     Name = name;
        //     Capacity = capacity > 0 ? capacity : null;
        //     Location = location;
        // }
        public ParkingLotEntity()
        {
        }
        public int Id { get; set; }

        public string Name { get; set; }

        public int? Capacity { get; set; }

        public string Location { get; set; }
    }
}
