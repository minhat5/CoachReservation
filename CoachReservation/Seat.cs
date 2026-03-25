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
        private string seatType;

        public Seat(string seatCode, string seatType)
        {
            this.seatCode = seatCode;
            this.seatType = seatType;
        }

        public Seat(SeatMap seatMap, string seatCode, int floor, int rowIndex, int columnIndex, string seatType)
        {
            this.seatMap = seatMap;
            this.seatCode = seatCode;
            this.floor = floor;
            this.rowIndex = rowIndex;
            this.columnIndex = columnIndex;
            this.seatType = seatType;
        }

        public Seat(int seatId, string seatCode, int floor, int rowIndex, int columnIndex, string seatType)
        {
            this.seatId = seatId;
            this.seatCode = seatCode;
            this.floor = floor;
            this.rowIndex = rowIndex;
            this.columnIndex = columnIndex;
            this.seatType = seatType;
        }

        public int SeatId { get => seatId; set => seatId = value; }
        public string SeatCode { get => seatCode; set => seatCode = value; }
        public int Floor { get => floor; set => floor = value; }
        public int RowIndex { get => rowIndex; set => rowIndex = value; }
        public int ColumnIndex { get => columnIndex; set => columnIndex = value; }
        public SeatMap SeatMap { get => seatMap; set => seatMap = value; }
        public string SeatType { get => seatType; set => seatType = value; }
    }
}