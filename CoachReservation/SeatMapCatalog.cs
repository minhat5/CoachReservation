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
        private string connectionString = "server=localhost;user=root;password=123456;database=coachreservationdb;";

        public SeatMapCatalog()
        {
        }

        public SeatMap GetSeatMapByVehicleId(int vehicleId)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT SeatMapId, VehicleId, Floors, GridColumns, GridRows FROM SeatMap WHERE VehicleId = @vehicleId";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@vehicleId", vehicleId);

                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        SeatMap seatMap = new SeatMap(reader.GetInt32(0), new Vehicle(vehicleId), reader.GetInt32(2), reader.GetInt32(3), reader.GetInt32(4));
                        reader.Close();
                        return seatMap;
                    }
                    reader.Close();
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public int SaveSeatMap(SeatMap seatMap)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Check if seat map already exists
                    string checkQuery = "SELECT SeatMapId FROM SeatMap WHERE VehicleId = @vehicleId";
                    MySqlCommand checkCmd = new MySqlCommand(checkQuery, connection);
                    checkCmd.Parameters.AddWithValue("@vehicleId", seatMap.Vehicle.VehicleId);
                    object result = checkCmd.ExecuteScalar();

                    if (result != null)
                    {
                        // Update existing seat map
                        int existingId = Convert.ToInt32(result);
                        string updateQuery = @"UPDATE SeatMap SET Floors = @floors, GridColumns = @gridColumns, GridRows = @gridRows 
                                          WHERE SeatMapId = @seatMapId";
                        MySqlCommand updateCmd = new MySqlCommand(updateQuery,  connection);
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
                        MySqlCommand insertCmd = new MySqlCommand(insertQuery, connection);
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
            }
            catch (Exception ex)
            {
                return -1;
            }
        }
    }
}
