using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace CoachReservation
{
    public class TripCatalog
    {
        private Database database;

        public TripCatalog(Database database)
        {
            this.database = database;
        }

        public List<Trip> SearchTrips(string departurePoint, string destination, DateTime departureDate)
        {
            List<Trip> trips = new List<Trip>();
            try
            {
                database.OpenDatabase();

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

                MySqlCommand cmd = new MySqlCommand(query, database.SqlConn);
                cmd.Parameters.AddWithValue("@departurePoint", departurePoint);
                cmd.Parameters.AddWithValue("@destination", destination);
                cmd.Parameters.AddWithValue("@departureDate", departureDate.Date);

                Console.WriteLine($"DEBUG: Searching trips with departure='{departurePoint}', destination='{destination}', date='{departureDate.Date:yyyy-MM-dd}', after current time");
                Console.WriteLine($"DEBUG: Current time: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");

                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Route route = new Route
                    {
                        RouteId = reader.GetInt32("RouteId"),
                        DeparturePoint = reader.GetString("DeparturePoint"),
                        Destination = reader.GetString("Destination")
                    };

                    Vehicle vehicle = new Vehicle
                    {
                        VehicleId = reader.GetInt32("VehicleId"),
                        LicensePlate = reader.GetString("LicensePlate"),
                        VehicleType = reader.GetString("VehicleType"),
                        TotalSeats = reader.GetInt32("TotalSeats")
                    };

                    Trip trip = new Trip
                    {
                        TripId = reader.GetInt32("TripId"),
                        DepartureDate = reader.GetDateTime("DepartureDate"),
                        DepartureTime = reader.GetTimeSpan("DepartureTime"),
                        BasePrice = reader.GetDecimal("BasePrice"),
                        Status = reader.GetString("Status"),
                        Route = route,
                        Vehicle = vehicle
                    };

                    trips.Add(trip);
                    Console.WriteLine($"DEBUG: Found trip {trip.TripId} - {trip.Route.DeparturePoint} -> {trip.Route.Destination} at {trip.DepartureTime:hh\\:mm\\:ss}");
                }

                Console.WriteLine($"DEBUG: Total trips found: {trips.Count}");
                reader.Close();
                return trips;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error searching trips: " + ex.Message);
                Console.WriteLine("Stack trace: " + ex.StackTrace);
                return new List<Trip>();
            }
            finally
            {
                database.CloseDatabase();
            }
        }

        public int GetEmptySeatsCount(int tripId)
        {
            try
            {
                database.OpenDatabase();

                string query = @"
                    SELECT v.TotalSeats - COUNT(ts.TripSeatId) as EmptySeats
                    FROM Trip t
                    LEFT JOIN Vehicle v ON t.VehicleId = v.VehicleId
                    LEFT JOIN TripSeat ts ON t.TripId = ts.TripId AND ts.Status = 'Đã đặt'
                    WHERE t.TripId = @tripId
                    GROUP BY v.TotalSeats";

                MySqlCommand cmd = new MySqlCommand(query, database.SqlConn);
                cmd.Parameters.AddWithValue("@tripId", tripId);

                object result = cmd.ExecuteScalar();
                int emptySeats = result != null && result != DBNull.Value ? Convert.ToInt32(result) : 0;

                database.CloseDatabase();
                return emptySeats;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error getting empty seats: " + ex.Message);
                return 0;
            }
        }
    }
}
