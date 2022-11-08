using System.Net;
using System;

namespace ParkingLotApi
{
    public class FullParkingLotException : Exception
    {
        public FullParkingLotException(string errorMessage) : base(errorMessage)
        {
            ErrorMessage = errorMessage;
            StatusCode = HttpStatusCode.Conflict;
        }

        public HttpStatusCode StatusCode { get; set; }
        public string ErrorMessage { get; set; }
    }
}
