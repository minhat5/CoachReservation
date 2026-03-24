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

        public List<Vehicle> GetAllVehicles()
        {
            List<Vehicle> vehicles = new List<Vehicle>();
            try
            {
                database.OpenDatabase();
                string query = "SELECT VehicleId, LicensePlate, VehicleType FROM Vehicle";
                MySqlCommand cmd = new MySqlCommand(query, database.SqlConn);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Vehicle vehicle = new Vehicle
                    {
                        VehicleId = reader.GetInt32("VehicleId"),
                        LicensePlate = reader.GetString("LicensePlate"),
                        VehicleType = reader.GetString("VehicleType")
                    };
                    vehicles.Add(vehicle);
                }

                reader.Close();
                return vehicles;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error getting vehicles: " + ex.Message);
                return new List<Vehicle>();
            }
            finally
            {
                database.CloseDatabase();
            }
        }

        public SeatMap GetSeatMapByVehicleId(int vehicleId)
        {
            try
            {
                database.OpenDatabase();
                string query = "SELECT SeatMapId, Floors, GridColumns, GridRows, VehicleId FROM SeatMap WHERE VehicleId = @vehicleId";
                MySqlCommand cmd = new MySqlCommand(query, database.SqlConn);
                cmd.Parameters.AddWithValue("@vehicleId", vehicleId);

                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    SeatMap seatMap = new SeatMap
                    {
                        SeatMapId = reader.GetInt32("SeatMapId"),
                        Floors = reader.GetInt32("Floors"),
                        GridColumns = reader.GetInt32("GridColumns"),
                        GridRows = reader.GetInt32("GridRows"),
                        Vehicle = new Vehicle { VehicleId = reader.GetInt32("VehicleId") }
                    };

                    reader.Close();
                    return seatMap;
                }

                reader.Close();
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error getting SeatMap: " + ex.Message);
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
                                FROM Seat WHERE SeatMapId = @seatMapId ORDER BY Floor, RowIndex, ColumnIndex";
                MySqlCommand cmd = new MySqlCommand(query, database.SqlConn);
                cmd.Parameters.AddWithValue("@seatMapId", seatMapId);

                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Seat seat = new Seat
                    {
                        SeatId = reader.GetInt32("SeatId"),
                        SeatCode = reader.GetString("SeatCode"),
                        Floor = reader.GetInt32("Floor"),
                        RowIndex = reader.GetInt32("RowIndex"),
                        ColumnIndex = reader.GetInt32("ColumnIndex"),
                        SeatType = reader.IsDBNull(5) ? "Gh?" : reader.GetString("SeatType")
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

        public int CreateOrGetSeatMapForVehicle(int vehicleId, int floors, int gridColumns, int gridRows)
        {
            try
            {
                database.OpenDatabase();

                // Check if SeatMap exists
                string checkQuery = "SELECT SeatMapId FROM SeatMap WHERE VehicleId = @vehicleId";
                MySqlCommand checkCmd = new MySqlCommand(checkQuery, database.SqlConn);
                checkCmd.Parameters.AddWithValue("@vehicleId", vehicleId);
                object result = checkCmd.ExecuteScalar();

                if (result != null && result != DBNull.Value)
                {
                    int seatMapId = (int)result;
                    // Update existing SeatMap
                    string updateQuery = "UPDATE SeatMap SET Floors = @floors, GridColumns = @gridColumns, GridRows = @gridRows WHERE SeatMapId = @seatMapId";
                    MySqlCommand updateCmd = new MySqlCommand(updateQuery, database.SqlConn);
                    updateCmd.Parameters.AddWithValue("@seatMapId", seatMapId);
                    updateCmd.Parameters.AddWithValue("@floors", floors);
                    updateCmd.Parameters.AddWithValue("@gridColumns", gridColumns);
                    updateCmd.Parameters.AddWithValue("@gridRows", gridRows);
                    updateCmd.ExecuteNonQuery();
                    return seatMapId;
                }
                else
                {
                    // Insert new SeatMap
                    string insertQuery = "INSERT INTO SeatMap (VehicleId, Floors, GridColumns, GridRows) VALUES (@vehicleId, @floors, @gridColumns, @gridRows)";
                    MySqlCommand insertCmd = new MySqlCommand(insertQuery, database.SqlConn);
                    insertCmd.Parameters.AddWithValue("@vehicleId", vehicleId);
                    insertCmd.Parameters.AddWithValue("@floors", floors);
                    insertCmd.Parameters.AddWithValue("@gridColumns", gridColumns);
                    insertCmd.Parameters.AddWithValue("@gridRows", gridRows);
                    insertCmd.ExecuteNonQuery();
                    return (int)insertCmd.LastInsertedId;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error creating/getting SeatMap: " + ex.Message);
                return -1;
            }
            finally
            {
                database.CloseDatabase();
            }
        }

        public bool SaveSingleSeat(int seatMapId, Seat seat)
        {
            try
            {
                database.OpenDatabase();

                // Check if seat exists
                string checkQuery = "SELECT SeatId FROM Seat WHERE SeatMapId = @seatMapId AND Floor = @floor AND RowIndex = @rowIndex AND ColumnIndex = @columnIndex";
                MySqlCommand checkCmd = new MySqlCommand(checkQuery, database.SqlConn);
                checkCmd.Parameters.AddWithValue("@seatMapId", seatMapId);
                checkCmd.Parameters.AddWithValue("@floor", seat.Floor);
                checkCmd.Parameters.AddWithValue("@rowIndex", seat.RowIndex);
                checkCmd.Parameters.AddWithValue("@columnIndex", seat.ColumnIndex);
                object result = checkCmd.ExecuteScalar();

                if (result != null && result != DBNull.Value)
                {
                    // Update existing seat
                    int seatId = (int)result;
                    string updateQuery = "UPDATE Seat SET SeatCode = @seatCode, SeatType = @seatType WHERE SeatId = @seatId";
                    MySqlCommand updateCmd = new MySqlCommand(updateQuery, database.SqlConn);
                    updateCmd.Parameters.AddWithValue("@seatId", seatId);
                    updateCmd.Parameters.AddWithValue("@seatCode", seat.SeatCode);
                    updateCmd.Parameters.AddWithValue("@seatType", seat.SeatType ?? "Gh?");
                    updateCmd.ExecuteNonQuery();
                }
                else
                {
                    // Insert new seat
                    string insertQuery = "INSERT INTO Seat (SeatMapId, SeatCode, Floor, RowIndex, ColumnIndex, SeatType) VALUES (@seatMapId, @seatCode, @floor, @rowIndex, @columnIndex, @seatType)";
                    MySqlCommand insertCmd = new MySqlCommand(insertQuery, database.SqlConn);
                    insertCmd.Parameters.AddWithValue("@seatMapId", seatMapId);
                    insertCmd.Parameters.AddWithValue("@seatCode", seat.SeatCode);
                    insertCmd.Parameters.AddWithValue("@floor", seat.Floor);
                    insertCmd.Parameters.AddWithValue("@rowIndex", seat.RowIndex);
                    insertCmd.Parameters.AddWithValue("@columnIndex", seat.ColumnIndex);
                    insertCmd.Parameters.AddWithValue("@seatType", seat.SeatType ?? "Gh?");
                    insertCmd.ExecuteNonQuery();
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error saving seat: " + ex.Message);
                return false;
            }
            finally
            {
                database.CloseDatabase();
            }
        }

        public void SaveSeatMapConfiguration(int vehicleId, int floors, int gridColumns, int gridRows, List<Seat> seats)
        {
            try
            {
                database.OpenDatabase();

                // Check if SeatMap exists
                string checkQuery = "SELECT SeatMapId FROM SeatMap WHERE VehicleId = @vehicleId";
                MySqlCommand checkCmd = new MySqlCommand(checkQuery, database.SqlConn);
                checkCmd.Parameters.AddWithValue("@vehicleId", vehicleId);
                object result = checkCmd.ExecuteScalar();

                int seatMapId;

                if (result != null && result != DBNull.Value)
                {
                    seatMapId = (int)result;
                    // Update existing SeatMap
                    string updateQuery = "UPDATE SeatMap SET Floors = @floors, GridColumns = @gridColumns, GridRows = @gridRows WHERE SeatMapId = @seatMapId";
                    MySqlCommand updateCmd = new MySqlCommand(updateQuery, database.SqlConn);
                    updateCmd.Parameters.AddWithValue("@seatMapId", seatMapId);
                    updateCmd.Parameters.AddWithValue("@floors", floors);
                    updateCmd.Parameters.AddWithValue("@gridColumns", gridColumns);
                    updateCmd.Parameters.AddWithValue("@gridRows", gridRows);
                    updateCmd.ExecuteNonQuery();

                    // Delete existing seats
                    string deleteQuery = "DELETE FROM Seat WHERE SeatMapId = @seatMapId";
                    MySqlCommand deleteCmd = new MySqlCommand(deleteQuery, database.SqlConn);
                    deleteCmd.Parameters.AddWithValue("@seatMapId", seatMapId);
                    deleteCmd.ExecuteNonQuery();
                }
                else
                {
                    // Insert new SeatMap
                    string insertQuery = "INSERT INTO SeatMap (VehicleId, Floors, GridColumns, GridRows) VALUES (@vehicleId, @floors, @gridColumns, @gridRows)";
                    MySqlCommand insertCmd = new MySqlCommand(insertQuery, database.SqlConn);
                    insertCmd.Parameters.AddWithValue("@vehicleId", vehicleId);
                    insertCmd.Parameters.AddWithValue("@floors", floors);
                    insertCmd.Parameters.AddWithValue("@gridColumns", gridColumns);
                    insertCmd.Parameters.AddWithValue("@gridRows", gridRows);
                    insertCmd.ExecuteNonQuery();

                    seatMapId = (int)insertCmd.LastInsertedId;
                }

                // Insert seats
                foreach (Seat seat in seats)
                {
                    string insertSeatQuery = "INSERT INTO Seat (SeatMapId, SeatCode, Floor, RowIndex, ColumnIndex, SeatType) VALUES (@seatMapId, @seatCode, @floor, @rowIndex, @columnIndex, @seatType)";
                    MySqlCommand insertSeatCmd = new MySqlCommand(insertSeatQuery, database.SqlConn);
                    insertSeatCmd.Parameters.AddWithValue("@seatMapId", seatMapId);
                    insertSeatCmd.Parameters.AddWithValue("@seatCode", seat.SeatCode);
                    insertSeatCmd.Parameters.AddWithValue("@floor", seat.Floor);
                    insertSeatCmd.Parameters.AddWithValue("@rowIndex", seat.RowIndex);
                    insertSeatCmd.Parameters.AddWithValue("@columnIndex", seat.ColumnIndex);
                    insertSeatCmd.Parameters.AddWithValue("@seatType", seat.SeatType ?? "Gh?");
                    insertSeatCmd.ExecuteNonQuery();
                }

                Console.WriteLine("SeatMap configuration saved successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error saving SeatMap configuration: " + ex.Message);
            }
            finally
            {
                database.CloseDatabase();
            }
        }
    }
}
