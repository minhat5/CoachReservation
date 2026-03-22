using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoachReservation
{
    public class TripSeat
    {
        private int tripSeatId;
        private Trip trip;
        private Seat seat;
        private string status;

        public int TripSeatId { get => tripSeatId; set => tripSeatId = value; }
        public string Status { get => status; set => status = value; }
        public Trip Trip { get => trip; set => trip = value; }
        public Seat Seat { get => seat; set => seat = value; }
    }
}