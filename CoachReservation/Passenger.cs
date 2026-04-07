using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoachReservation
{
    public class Passenger
    {
        private int passengerId;
        private string fullName;
        private string phoneNumber;

        public Passenger(int passengerId, string fullName, string phoneNumber)
        {
            this.passengerId = passengerId;
            this.fullName = fullName;
            this.phoneNumber = phoneNumber;
        }

        public int PassengerId { get => passengerId; set => passengerId = value; }
        public string FullName { get => fullName; set => fullName = value; }
        public string PhoneNumber { get => phoneNumber; set => phoneNumber = value; }
    }
}