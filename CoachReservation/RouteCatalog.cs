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
        private Database database;
        public RouteCatalog(Database database)
        {
            this.database = database;
        }

        public List<string> GetUniqueDeparturePoints()
        {
            List<string> departurePoints = new List<string>();
            try
            {
                database.OpenDatabase();

                string query = "SELECT DISTINCT DeparturePoint FROM Route ORDER BY DeparturePoint";

                MySqlCommand cmd = new MySqlCommand(query, database.SqlConn);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    departurePoints.Add(reader.GetString("DeparturePoint"));
                }

                reader.Close();
                return departurePoints;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error getting departure points: " + ex.Message);
                return new List<string>();
            }
            finally
            {
                database.CloseDatabase();
            }
        }

        public List<string> GetDestinationsByDeparture(string departurePoint)
        {
            List<string> destinations = new List<string>();
            try
            {
                database.OpenDatabase();

                string query = "SELECT DISTINCT Destination FROM Route WHERE DeparturePoint = @departurePoint ORDER BY Destination";

                MySqlCommand cmd = new MySqlCommand(query, database.SqlConn);
                cmd.Parameters.AddWithValue("@departurePoint", departurePoint);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    destinations.Add(reader.GetString("Destination"));
                }

                reader.Close();
                return destinations;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error getting destinations: " + ex.Message);
                return new List<string>();
            }
            finally
            {
                database.CloseDatabase();
            }
        }
    }
}
