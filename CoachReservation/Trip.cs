using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoachReservation
{
    public class Trip
    {
        private int tripId;
        private int routeId;
        private int vehicleId;
        private System.DateTime departureDate;
        private System.TimeSpan departureTime;
        private decimal basePrice;
        private string status;
    }
}