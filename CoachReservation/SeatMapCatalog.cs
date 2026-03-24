using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoachReservation
{
    public class SeatMapCatalog
    {
        private Database database;

        public SeatMapCatalog(Database database)
        {
            this.database = database;
        }

        public SeatMap GetSeatMapByVehicleId(int vehicleId)
        {
            try
            {
                database.OpenDatabase();
                string query = "SELECT SeatMapId, VehicleId, Floors, GridColumns, GridRows FROM SeatMap WHERE VehicleId = @vehicleId";
                MySqlCommand cmd = new MySqlCommand(query, database.SqlConn);
                cmd.Parameters.AddWithValue("@vehicleId", vehicleId);

                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    SeatMap seatMap = new SeatMap
                    {
                        SeatMapId = reader.GetInt32(0),
                        Floors = reader.GetInt32(2),
                        GridColumns = reader.GetInt32(3),
                        GridRows = reader.GetInt32(4),
                        Vehicle = new Vehicle { VehicleId = vehicleId }
                    };
                    reader.Close();
                    return seatMap;
                }
                reader.Close();
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error getting seat map: " + ex.Message);
                return null;
            }
            finally
            {
                database.CloseDatabase();
            }
        }

        public List<Seat> GetSeatsBySeatMapId(int seatMapId)
        {
            List<Seat> seats = new List<Seat>();
            try
            {
                database.OpenDatabase();
                string query = @"SELECT SeatId, SeatCode, Floor, RowIndex, ColumnIndex, SeatType 
                               FROM Seat WHERE SeatMapId = @seatMapId";
                MySqlCommand cmd = new MySqlCommand(query, database.SqlConn);
                cmd.Parameters.AddWithValue("@seatMapId", seatMapId);

                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Seat seat = new Seat
                    {
                        SeatId = reader.GetInt32(0),
                        SeatCode = reader.IsDBNull(1) ? "" : reader.GetString(1),
                        Floor = reader.GetInt32(2),
                        RowIndex = reader.GetInt32(3),
                        ColumnIndex = reader.GetInt32(4),
                        SeatType = reader.IsDBNull(5) ? "" : reader.GetString(5)
                    };
                    seats.Add(seat);
                }
                reader.Close();
                return seats;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error getting seats: " + ex.Message);
                return new List<Seat>();
            }
            finally
            {
                database.CloseDatabase();
            }
        }

        public int SaveSeatMap(SeatMap seatMap)
        {
            try
            {
                database.OpenDatabase();
                
                // Check if seat map already exists
                string checkQuery = "SELECT SeatMapId FROM SeatMap WHERE VehicleId = @vehicleId";
                MySqlCommand checkCmd = new MySqlCommand(checkQuery, database.SqlConn);
                checkCmd.Parameters.AddWithValue("@vehicleId", seatMap.Vehicle.VehicleId);
                object result = checkCmd.ExecuteScalar();

                if (result != null)
                {
                    // Update existing seat map
                    int existingId = Convert.ToInt32(result);
                    string updateQuery = @"UPDATE SeatMap SET Floors = @floors, GridColumns = @gridColumns, GridRows = @gridRows 
                                          WHERE SeatMapId = @seatMapId";
                    MySqlCommand updateCmd = new MySqlCommand(updateQuery, database.SqlConn);
                    updateCmd.Parameters.AddWithValue("@floors", seatMap.Floors);
                    updateCmd.Parameters.AddWithValue("@gridColumns", seatMap.GridColumns);
                    updateCmd.Parameters.AddWithValue("@gridRows", seatMap.GridRows);
                    updateCmd.Parameters.AddWithValue("@seatMapId", existingId);
                    updateCmd.ExecuteNonQuery();
                    return existingId;
                }
                else
                {
                    // Insert new seat map
                    string insertQuery = @"INSERT INTO SeatMap (VehicleId, Floors, GridColumns, GridRows) 
                                          VALUES (@vehicleId, @floors, @gridColumns, @gridRows)";
                    MySqlCommand insertCmd = new MySqlCommand(insertQuery, database.SqlConn);
                    insertCmd.Parameters.AddWithValue("@vehicleId", seatMap.Vehicle.VehicleId);
                    insertCmd.Parameters.AddWithValue("@floors", seatMap.Floors);
                    insertCmd.Parameters.AddWithValue("@gridColumns", seatMap.GridColumns);
                    insertCmd.Parameters.AddWithValue("@gridRows", seatMap.GridRows);

                    insertCmd.ExecuteNonQuery();
                    
                    // Get the inserted SeatMapId
                    long seatMapId = insertCmd.LastInsertedId;
                    return (int)seatMapId;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error saving seat map: " + ex.Message);
                return -1;
            }
            finally
            {
                database.CloseDatabase();
            }
        }

        public void SaveSeats(List<Seat> seats)
        {
            try
            {
                database.OpenDatabase();
                foreach (var seat in seats)
                {
                    // Check if seat already exists
                    string checkQuery = @"SELECT SeatId FROM Seat 
                                         WHERE SeatMapId = @seatMapId AND Floor = @floor 
                                         AND RowIndex = @rowIndex AND ColumnIndex = @columnIndex";
                    MySqlCommand checkCmd = new MySqlCommand(checkQuery, database.SqlConn);
                    checkCmd.Parameters.AddWithValue("@seatMapId", seat.SeatMap.SeatMapId);
                    checkCmd.Parameters.AddWithValue("@floor", seat.Floor);
                    checkCmd.Parameters.AddWithValue("@rowIndex", seat.RowIndex);
                    checkCmd.Parameters.AddWithValue("@columnIndex", seat.ColumnIndex);

                    object result = checkCmd.ExecuteScalar();

                    if (result != null)
                    {
                        // Update existing seat
                        int seatId = Convert.ToInt32(result);
                        string updateQuery = @"UPDATE Seat SET SeatCode = @seatCode, SeatType = @seatType 
                                            WHERE SeatId = @seatId";
                        MySqlCommand updateCmd = new MySqlCommand(updateQuery, database.SqlConn);
                        updateCmd.Parameters.AddWithValue("@seatCode", seat.SeatCode ?? "");
                        updateCmd.Parameters.AddWithValue("@seatType", seat.SeatType ?? "");
                        updateCmd.Parameters.AddWithValue("@seatId", seatId);
                        updateCmd.ExecuteNonQuery();
                    }
                    else
                    {
                        // Insert new seat
                        string insertQuery = @"INSERT INTO Seat (SeatMapId, SeatCode, Floor, RowIndex, ColumnIndex, SeatType) 
                                             VALUES (@seatMapId, @seatCode, @floor, @rowIndex, @columnIndex, @seatType)";
                        MySqlCommand insertCmd = new MySqlCommand(insertQuery, database.SqlConn);
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
            catch (Exception ex)
            {
                Console.WriteLine("Error saving seats: " + ex.Message);
            }
            finally
            {
                database.CloseDatabase();
            }
        }
    }
}
