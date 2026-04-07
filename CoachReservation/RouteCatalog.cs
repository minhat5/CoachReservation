using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace CoachReservation
{
    public class RouteCatalog
    {
        private string connectionString = "server=localhost;user=root;password=123456;database=coachreservationdb;";
        public RouteCatalog(){}

        public List<string> GetUniqueDeparturePoints()
        {
            List<string> departurePoints = new List<string>();

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT DISTINCT DeparturePoint FROM Route ORDER BY DeparturePoint";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                departurePoints.Add(reader.GetString("DeparturePoint"));
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error getting departure points: " + ex.Message);
                }
                return departurePoints;
            }
        }

        public List<string> GetDestinationsByDeparture(string departurePoint)
        {
            List<string> destinations = new List<string>();

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT DISTINCT Destination FROM Route WHERE DeparturePoint = @departurePoint ORDER BY Destination";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@departurePoint", departurePoint);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                destinations.Add(reader.GetString("Destination"));
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error getting destinations: " + ex.Message);
                }
            }
            return destinations;
        }
    }
}
