using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace CoachReservation
{
    public class TicketCatalog
    {
        private Database database;

        public TicketCatalog()
        {
            database = new Database();
        }

        public void UpdateTicketStatus(int ticketId, string newStatus)
        {
            try
            {
                database.OpenDatabase();
                string query = "UPDATE Ticket SET TicketStatus = @newStatus WHERE TicketId = @ticketId";
                MySqlCommand cmd = new MySqlCommand(query, database.SqlConn);
                cmd.Parameters.AddWithValue("@newStatus", newStatus);
                cmd.Parameters.AddWithValue("@ticketId", ticketId);
                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    Console.WriteLine("Ticket status updated successfully.");
                }
                else
                {
                    Console.WriteLine("No ticket found with the provided ID.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error updating ticket status: " + ex.Message);
            }
            finally
            {
                database.CloseDatabase();
            }
        }
        public List<string> GetSeatsByTicket(int ticketId)
        {
            List<string> seats = new List<string>();
            try
            {
                database.OpenDatabase();

                string query = @"
                    SELECT s.SeatCode
                    FROM TicketDetail td
                    LEFT JOIN TripSeat ts ON td.TripSeatId = ts.TripSeatId
                    LEFT JOIN Seat s ON ts.SeatId = s.SeatId
                    WHERE td.TicketId = @ticketId
                    ORDER BY s.SeatCode";

                MySqlCommand cmd = new MySqlCommand(query, database.SqlConn);
                cmd.Parameters.AddWithValue("@ticketId", ticketId);

                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    if (!reader.IsDBNull(0))
                    {
                        seats.Add(reader.GetString("SeatCode"));
                    }
                }

                reader.Close();
                return seats;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error getting seats: " + ex.Message);
                return new List<string>();
            }
            finally
            {
                database.CloseDatabase();
            }
        }

        public Ticket FindTicket(int ticketId)
        {
            try
            {
                database.OpenDatabase();

                string query = @"
                    SELECT t.TicketId, t.TripId, t.PassengerId, t.BookingDate, t.TotalAmount, t.TicketStatus,
                           tr.TripId AS TripId_Trip, tr.RouteId, tr.VehicleId, tr.DepartureDate, tr.DepartureTime, tr.BasePrice, tr.Status,
                           r.RouteId AS RouteId_Route, r.DeparturePoint, r.Destination,
                           v.VehicleId AS VehicleId_Vehicle, v.LicensePlate, v.VehicleType, v.TotalSeats,
                           p.PassengerId AS PassengerId_Passenger, p.FullName, p.PhoneNumber
                    FROM Ticket t
                    LEFT JOIN Trip tr ON t.TripId = tr.TripId
                    LEFT JOIN Route r ON tr.RouteId = r.RouteId
                    LEFT JOIN Vehicle v ON tr.VehicleId = v.VehicleId
                    LEFT JOIN Passenger p ON t.PassengerId = p.PassengerId
                    WHERE t.TicketId = @ticketId";

                MySqlCommand cmd = new MySqlCommand(query, database.SqlConn);
                cmd.Parameters.AddWithValue("@ticketId", ticketId);

                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    int readTicketId = reader.GetInt32("TicketId");
                    DateTime bookingDate = reader.GetDateTime("BookingDate");
                    decimal totalAmount = reader.GetDecimal("TotalAmount");
                    string ticketStatus = reader.GetString("TicketStatus");

                    Route route = null;
                    int routeIdOrd = reader.GetOrdinal("RouteId_Route");
                    if (!reader.IsDBNull(routeIdOrd))
                    {
                        int routeId = reader.GetInt32(routeIdOrd);
                        string departurePoint = reader.GetString("DeparturePoint");
                        string destination = reader.GetString("Destination");

                        route = new Route
                        {
                            RouteId = routeId,
                            DeparturePoint = departurePoint,
                            Destination = destination
                        };
                    }

                    Vehicle vehicle = null;
                    int vehicleIdOrd = reader.GetOrdinal("VehicleId_Vehicle");
                    if (!reader.IsDBNull(vehicleIdOrd))
                    {
                        int vehicleId = reader.GetInt32(vehicleIdOrd);
                        string licensePlate = reader.GetString("LicensePlate");
                        string vehicleType = reader.GetString("VehicleType");
                        int totalSeats = reader.GetInt32("TotalSeats");

                        vehicle = new Vehicle
                        {
                            VehicleId = vehicleId,
                            LicensePlate = licensePlate,
                            VehicleType = vehicleType,
                            TotalSeats = totalSeats
                        };
                    }

                    Trip trip = null;
                    int tripIdOrd = reader.GetOrdinal("TripId_Trip");
                    if (!reader.IsDBNull(tripIdOrd))
                    {
                        int tripId = reader.GetInt32(tripIdOrd);
                        DateTime departureDate = reader.GetDateTime("DepartureDate");
                        TimeSpan departureTime = reader.GetTimeSpan("DepartureTime");
                        decimal basePrice = reader.GetDecimal("BasePrice");
                        string tripStatus = reader.GetString("Status");

                        trip = new Trip
                        {
                            TripId = tripId,
                            DepartureDate = departureDate,
                            DepartureTime = departureTime,
                            BasePrice = basePrice,
                            Status = tripStatus,
                            Route = route,
                            Vehicle = vehicle
                        };
                    }

                    Passenger passenger = null;
                    int passengerIdOrd = reader.GetOrdinal("PassengerId_Passenger");
                    if (!reader.IsDBNull(passengerIdOrd))
                    {
                        int psgId = reader.GetInt32(passengerIdOrd);
                        string fullName = reader.GetString("FullName");
                        string phoneNumber = reader.GetString("PhoneNumber");

                        passenger = new Passenger
                        {
                            PassengerId = psgId,
                            FullName = fullName,
                            PhoneNumber = phoneNumber
                        };
                    }

                    reader.Close();

                    return new Ticket(readTicketId, trip, passenger, bookingDate, totalAmount, ticketStatus);
                }

                reader.Close();
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error finding ticket: " + ex.Message);
                return null;
            }
            finally
            {
                database.CloseDatabase();
            }
        }
    }
}
