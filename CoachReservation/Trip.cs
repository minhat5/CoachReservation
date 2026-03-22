using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoachReservation
{
    public class Trip
    {
        private int tripId;
        private Route route;
        private Vehicle vehicle;
        private DateTime departureDate;
        private TimeSpan departureTime;
        private decimal basePrice;
        private string status;

        public int TripId { get => tripId; set => tripId = value; }
        public DateTime DepartureDate { get => departureDate; set => departureDate = value; }
        public TimeSpan DepartureTime { get => departureTime; set => departureTime = value; }
        public decimal BasePrice { get => basePrice; set => basePrice = value; }
        public string Status { get => status; set => status = value; }
        public Route Route { get => route; set => route = value; }
        public Vehicle Vehicle { get => vehicle; set => vehicle = value; }
    }
}