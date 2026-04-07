using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoachReservation
{
    public class VehicleCatalog
    {
        public VehicleCatalog()
        {
        }

        public List<Vehicle> GetAllVehicles()
        {
            List<Vehicle> vehicles = new List<Vehicle>();
            try
            {
                using (MySqlConnection connection = new MySqlConnection("server=localhost;user=root;password=123456;database=coachreservationdb;"))
                {
                    connection.Open();
                    string query = "SELECT VehicleId, LicensePlate, VehicleType FROM Vehicle";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Vehicle vehicle = new Vehicle(reader.GetInt32("VehicleId"), reader.GetString("LicensePlate"), reader.GetString("VehicleType"));
                        vehicles.Add(vehicle);
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error getting vehicles: " + ex.Message);
            }

            return vehicles;
        }

        public int GetMaxSeats()
        {
            int maxSeats = 0;
            try
            {
                using (MySqlConnection connection = new MySqlConnection("server=localhost;user=root;password=123456;database=coachreservationdb;"))
                {
                    connection.Open();
                    string query = "SELECT MAX(TotalSeats) as MaxSeats FROM Vehicle";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    object result = cmd.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        maxSeats = Convert.ToInt32(result);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error getting max seats: " + ex.Message);
            }
            return maxSeats;
        }

        public Vehicle GetVehicleById(int vehicleId)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection("server=localhost;user=root;password=123456;database=coachreservationdb;"))
                {
                    connection.Open();
                    string query = "SELECT VehicleId, LicensePlate, VehicleType FROM Vehicle WHERE VehicleId = @vehicleId";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@vehicleId", vehicleId);

                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        Vehicle vehicle = new Vehicle(reader.GetInt32("VehicleId"), reader.GetString("LicensePlate"), reader.GetString("VehicleType"));
                        reader.Close();
                        return vehicle;
                    }
                    reader.Close();
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error getting vehicle: " + ex.Message);
                return null;
            }
        }
    }
}
