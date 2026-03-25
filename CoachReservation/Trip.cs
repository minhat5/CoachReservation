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

        public Trip(int tripId, Route route, Vehicle vehicle, DateTime departureDate, TimeSpan departureTime, decimal basePrice, string status)
        {
            this.tripId = tripId;
            this.route = route;
            this.vehicle = vehicle;
            this.departureDate = departureDate;
            this.departureTime = departureTime;
            this.basePrice = basePrice;
            this.status = status;
        }

        public int TripId { get => tripId; set => tripId = value; }
        public DateTime DepartureDate { get => departureDate; set => departureDate = value; }
        public TimeSpan DepartureTime { get => departureTime; set => departureTime = value; }
        public decimal BasePrice { get => basePrice; set => basePrice = value; }
        public string Status { get => status; set => status = value; }
        public Route Route { get => route; set => route = value; }
        public Vehicle Vehicle { get => vehicle; set => vehicle = value; }
    }
}