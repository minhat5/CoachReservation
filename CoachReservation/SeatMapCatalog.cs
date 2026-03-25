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
                    SeatMap seatMap = new SeatMap(reader.GetInt32(0), new Vehicle(vehicleId), reader.GetInt32(2), reader.GetInt32(3), reader.GetInt32(4));
                    reader.Close();
                    return seatMap;
                }
                reader.Close();
                return null;
            }
            catch (Exception ex)
            {
                return null;
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
                return -1;
            }
            finally
            {
                database.CloseDatabase();
            }
        }
    }
}
