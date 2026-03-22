using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoachReservation
{
    public class Seat
    {
        private int seatId;
        private SeatMap seatMap;
        private string seatCode;
        private int floor;
        private int rowIndex;
        private int columnIndex;

        public int SeatId { get => seatId; set => seatId = value; }
        public string SeatCode { get => seatCode; set => seatCode = value; }
        public int Floor { get => floor; set => floor = value; }
        public int RowIndex { get => rowIndex; set => rowIndex = value; }
        public int ColumnIndex { get => columnIndex; set => columnIndex = value; }
        public SeatMap SeatMap { get => seatMap; set => seatMap = value; }
    }
}