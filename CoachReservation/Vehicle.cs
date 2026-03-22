using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoachReservation
{
    public class Vehicle
    {
        private int vehicleId;
        private string licensePlate;
        private string vehicleType;
        private int totalSeats;

        public int VehicleId { get => vehicleId; set => vehicleId = value; }
        public string LicensePlate { get => licensePlate; set => licensePlate = value; }
        public string VehicleType { get => vehicleType; set => vehicleType = value; }
        public int TotalSeats { get => totalSeats; set => totalSeats = value; }
    }
}