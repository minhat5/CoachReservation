using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoachReservation
{
    public class TicketDetail
    {
        private int ticketDetailId;
        private Ticket ticket;
        private TripSeat tripSeat;
        private decimal price;

        public int TicketDetailId { get => ticketDetailId; set => ticketDetailId = value; }
        public Ticket Ticket { get => ticket; set => ticket = value; }
        public TripSeat TripSeat { get => tripSeat; set => tripSeat = value; }
        public decimal Price { get => price; set => price = value; }
    }
}