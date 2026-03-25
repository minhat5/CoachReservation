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

        public SeatMap(Vehicle vehicle, int floors, int gridColumns, int gridRows)
        {
            this.vehicle = vehicle;
            this.floors = floors;
            this.gridColumns = gridColumns;
            this.gridRows = gridRows;
        }

        public SeatMap(int seatMapId, Vehicle vehicle, int floors, int gridColumns, int gridRows)
        {
            this.seatMapId = seatMapId;
            this.vehicle = vehicle;
            this.floors = floors;
            this.gridColumns = gridColumns;
            this.gridRows = gridRows;
        }

        public int SeatMapId { get => seatMapId; set => seatMapId = value; }
        public int Floors { get => floors; set => floors = value; }
        public int GridColumns { get => gridColumns; set => gridColumns = value; }
        public int GridRows { get => gridRows; set => gridRows = value; }
        public Vehicle Vehicle { get => vehicle; set => vehicle = value; }
    }
}