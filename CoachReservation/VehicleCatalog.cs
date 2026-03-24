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
        private Database database;
        public VehicleCatalog(Database database)
        {
            this.database = database;
        }

        public int GetMaxSeats()
        {
            int maxSeats = 0;
            try
            {
                database.OpenDatabase();
                string query = "SELECT MAX(TotalSeats) as MaxSeats FROM Vehicle";
                MySqlCommand cmd = new MySqlCommand(query, database.SqlConn);
                object result = cmd.ExecuteScalar();

                if (result != null && result != DBNull.Value)
                {
                    maxSeats = Convert.ToInt32(result);
                }

                database.CloseDatabase();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error getting max seats: " + ex.Message);
            }
            return maxSeats;
        }
    }
}
