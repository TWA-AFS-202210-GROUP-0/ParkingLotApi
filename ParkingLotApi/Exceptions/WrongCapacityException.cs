using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingLotApi.Exceptions
{
    public class WrongCapacityException : Exception
    {
        public WrongCapacityException(string message) : base(message)
        {
        }
    }
}
