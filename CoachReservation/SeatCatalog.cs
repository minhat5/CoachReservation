using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoachReservation
{
    public class SeatCatalog
    {
        public SeatCatalog()
        {}

        public List<Seat> GetSeatsBySeatMapId(int seatMapId)
        {
            List<Seat> seats = new List<Seat>();
            try
            {
                using (MySqlConnection connection = new MySqlConnection("server=localhost;user=root;password=123456;database=coachreservationdb;"))
                {
                    connection.Open();
                    string query = @"SELECT SeatId, SeatCode, Floor, RowIndex, ColumnIndex, SeatType 
                               FROM Seat WHERE SeatMapId = @seatMapId";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@seatMapId", seatMapId);

                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Seat seat = new Seat(reader.GetInt32(0), reader.IsDBNull(1) ? "" : reader.GetString(1), reader.GetInt32(2), reader.GetInt32(3), reader.GetInt32(4), reader.IsDBNull(5) ? "" : reader.GetString(5));
                        seats.Add(seat);
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
            }
            return seats;
        }

        public void SaveSeats(List<Seat> seats)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection("server=localhost;user=root;password=123456;database=coachreservationdb;"))
                {
                    connection.Open();
                    foreach (var seat in seats)
                    {
                        string checkQuery = @"SELECT SeatId FROM Seat 
                                         WHERE SeatMapId = @seatMapId AND Floor = @floor 
                                         AND RowIndex = @rowIndex AND ColumnIndex = @columnIndex";
                        MySqlCommand checkCmd = new MySqlCommand(checkQuery, connection);
                        checkCmd.Parameters.AddWithValue("@seatMapId", seat.SeatMap.SeatMapId);
                        checkCmd.Parameters.AddWithValue("@floor", seat.Floor);
                        checkCmd.Parameters.AddWithValue("@rowIndex", seat.RowIndex);
                        checkCmd.Parameters.AddWithValue("@columnIndex", seat.ColumnIndex);

                        object result = checkCmd.ExecuteScalar();

                        if (result != null)
                        {
                            int seatId = Convert.ToInt32(result);
                            string updateQuery = @"UPDATE Seat SET SeatCode = @seatCode, SeatType = @seatType 
                                            WHERE SeatId = @seatId";
                            MySqlCommand updateCmd = new MySqlCommand(updateQuery, connection);
                            updateCmd.Parameters.AddWithValue("@seatCode", seat.SeatCode ?? "");
                            updateCmd.Parameters.AddWithValue("@seatType", seat.SeatType ?? "");
                            updateCmd.Parameters.AddWithValue("@seatId", seatId);
                            updateCmd.ExecuteNonQuery();
                        }
                        else
                        {
                            string insertQuery = @"INSERT INTO Seat (SeatMapId, SeatCode, Floor, RowIndex, ColumnIndex, SeatType) 
                                             VALUES (@seatMapId, @seatCode, @floor, @rowIndex, @columnIndex, @seatType)";
                            MySqlCommand insertCmd = new MySqlCommand(insertQuery, connection);
                            insertCmd.Parameters.AddWithValue("@seatMapId", seat.SeatMap.SeatMapId);
                            insertCmd.Parameters.AddWithValue("@seatCode", seat.SeatCode ?? "");
                            insertCmd.Parameters.AddWithValue("@floor", seat.Floor);
                            insertCmd.Parameters.AddWithValue("@rowIndex", seat.RowIndex);
                            insertCmd.Parameters.AddWithValue("@columnIndex", seat.ColumnIndex);
                            insertCmd.Parameters.AddWithValue("@seatType", seat.SeatType ?? "");
                            insertCmd.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error saving seats: " + ex.Message);
                throw;
            }
        }

        public List<int> GetBookedSeatsForTrip(int tripId)
        {
            List<int> bookedSeats = new List<int>();
            try
            {
                using (MySqlConnection connection = new MySqlConnection("server=localhost;user=root;password=123456;database=coachreservationdb;"))
                {
                    connection.Open();

                    string query = @"SELECT s.SeatId FROM Seat s
                               INNER JOIN TripSeat ts ON s.SeatId = ts.SeatId
                               WHERE ts.TripId = @tripId AND ts.Status = 'Đã đặt'";

                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@tripId", tripId);

                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        bookedSeats.Add(reader.GetInt32(0));
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error getting booked seats: " + ex.Message);
            }
            return bookedSeats;
        }

        public string GetSeatCodesBySeatIds(List<int> seatIds)
        {
            if (seatIds == null || seatIds.Count == 0)
            {
                return string.Empty;
            }

            try
            {
                using (MySqlConnection connection = new MySqlConnection("server=localhost;user=root;password=123456;database=coachreservationdb;"))
                {
                    connection.Open();

                    string seatIdList = string.Join(",", seatIds);
                    string query = $@"SELECT GROUP_CONCAT(SeatCode SEPARATOR ', ') as Seats FROM Seat 
                                 WHERE SeatId IN ({seatIdList})";

                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    object result = cmd.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        return result.ToString();
                    }

                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
    }
}
