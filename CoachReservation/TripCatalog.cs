using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace CoachReservation
{
    public class TripCatalog
    {
        private string connectionString = "server=localhost;user=root;password=123456;database=coachreservationdb;";

        public TripCatalog()
        {
        }

        public List<Trip> SearchTrips(string departurePoint, string destination, DateTime departureDate)
        {
            List<Trip> trips = new List<Trip>();
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"
                    SELECT t.TripId, t.DepartureDate, t.DepartureTime, t.BasePrice, t.Status,
                           r.RouteId, r.DeparturePoint, r.Destination,
                           v.VehicleId, v.LicensePlate, v.VehicleType, v.TotalSeats
                    FROM Trip t
                    INNER JOIN Route r ON t.RouteId = r.RouteId
                    INNER JOIN Vehicle v ON t.VehicleId = v.VehicleId
                    WHERE r.DeparturePoint = @departurePoint
                    AND r.Destination = @destination
                    AND DATE(t.DepartureDate) = @departureDate
                    AND CONCAT(DATE(t.DepartureDate), ' ', TIME_FORMAT(t.DepartureTime, '%H:%i:%s')) > NOW()
                    ORDER BY t.DepartureTime";

                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@departurePoint", departurePoint);
                    cmd.Parameters.AddWithValue("@destination", destination);
                    cmd.Parameters.AddWithValue("@departureDate", departureDate.Date);

                    MySqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Route route = new Route(reader.GetInt32("RouteId"), reader.GetString("DeparturePoint"), reader.GetString("Destination"));

                        Vehicle vehicle = new Vehicle(reader.GetInt32("VehicleId"), reader.GetString("LicensePlate"), reader.GetString("VehicleType"), reader.GetInt32("TotalSeats"));

                        Trip trip = new Trip(reader.GetInt32("TripId"), route, vehicle, reader.GetDateTime("DepartureDate"), reader.GetTimeSpan("DepartureTime"), reader.GetDecimal("BasePrice"), reader.GetString("Status"));

                        trips.Add(trip);
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error searching trips: " + ex.Message);
            }
            return trips;
        }

        public int GetEmptySeatsCount(int tripId)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"
                    SELECT v.TotalSeats - COUNT(ts.TripSeatId) as EmptySeats
                    FROM Trip t
                    LEFT JOIN Vehicle v ON t.VehicleId = v.VehicleId
                    LEFT JOIN TripSeat ts ON t.TripId = ts.TripId AND ts.Status = 'Đã đặt'
                    WHERE t.TripId = @tripId
                    GROUP BY v.TotalSeats";

                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@tripId", tripId);

                    object result = cmd.ExecuteScalar();
                    int emptySeats = result != null && result != DBNull.Value ? Convert.ToInt32(result) : 0;

                    return emptySeats;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }
}
