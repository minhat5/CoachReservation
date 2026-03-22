using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoachReservation
{
    public class Ticket
    {
        private int ticketId;
        private Trip trip;
        private Passenger passenger;
        private DateTime bookingDate;
        private decimal totalAmount;
        private string ticketStatus;

        public int TicketId { get => ticketId; set => ticketId = value; }
        public DateTime BookingDate { get => bookingDate; set => bookingDate = value; }
        public decimal TotalAmount { get => totalAmount; set => totalAmount = value; }
        public string TicketStatus { get => ticketStatus; set => ticketStatus = value; }
        public Trip Trip { get => trip; set => trip = value; }
        public Passenger Passenger { get => passenger; set => passenger = value; }

        public Ticket(int ticketId, Trip trip, Passenger passenger, DateTime bookingDate, decimal totalAmount, string ticketStatus)
        {
            this.ticketId = ticketId;
            this.trip = trip;
            this.passenger = passenger;
            this.bookingDate = bookingDate;
            this.totalAmount = totalAmount;
            this.ticketStatus = ticketStatus;
        }
    }
}