using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoachReservation
{
    public class SeatMap
    {
        private int seatMapId;
        private Vehicle vehicle;
        private int floors;
        private int gridColumns;
        private int gridRows;

        public int SeatMapId { get => seatMapId; set => seatMapId = value; }
        public int Floors { get => floors; set => floors = value; }
        public int GridColumns { get => gridColumns; set => gridColumns = value; }
        public int GridRows { get => gridRows; set => gridRows = value; }
        public Vehicle Vehicle { get => vehicle; set => vehicle = value; }
    }
}