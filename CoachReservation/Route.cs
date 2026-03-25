using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoachReservation
{
    public class Route
    {
        private int routeId;
        private string departurePoint;
        private string destination;

        public Route(int routeId, string departurePoint, string destination)
        {
            this.routeId = routeId;
            this.departurePoint = departurePoint;
            this.destination = destination;
        }

        public int RouteId { get => routeId; set => routeId = value; }
        public string DeparturePoint { get => departurePoint; set => departurePoint = value; }
        public string Destination { get => destination; set => destination = value; }
    }
}